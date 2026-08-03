using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Domain;
using ERP.Api.Dtos;
using ERP.Api.Interfaces;

namespace ERP.Api.Services;

public class RoleService : IRoleService
{
    private readonly ErpDbContext _context;

    public RoleService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Where(r => r.Active)
            .ToListAsync();

        return roles.Select(MapToDto).ToList();
    }

    public async Task<RoleDto?> GetRoleByIdAsync(int id)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.ID == id && r.Active);

        return role != null ? MapToDto(role) : null;
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto dto)
    {
        var role = new Role
        {
            Name = dto.Name,
            Description = dto.Description,
            Active = true
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Assign permissions if provided
        if (dto.PermissionIds != null && dto.PermissionIds.Any())
        {
            await AssignPermissionsToRoleAsync(role.ID, dto.PermissionIds);
        }

        // Reload with permissions
        var createdRole = await GetRoleByIdAsync(role.ID);
        return createdRole!;
    }

    public async Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto dto)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null || !role.Active)
            throw new Exception("Role not found or inactive.");

        role.Name = dto.Name;
        role.Description = dto.Description;

        await _context.SaveChangesAsync();

        // Update permissions if provided
        if (dto.PermissionIds != null)
        {
            await AssignPermissionsToRoleAsync(id, dto.PermissionIds);
        }

        // Reload with permissions
        var updatedRole = await GetRoleByIdAsync(id);
        return updatedRole!;
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role == null)
            return false;

        role.Active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
    {
        var role = await _context.Roles.FindAsync(roleId);
        if (role == null)
            return false;

        // Remove existing permissions
        var existingPermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();
        _context.RolePermissions.RemoveRange(existingPermissions);

        // Add new permissions
        foreach (var permissionId in permissionIds)
        {
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission != null && permission.Active)
            {
                _context.RolePermissions.Add(new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                });
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private static RoleDto MapToDto(Role role)
    {
        return new RoleDto
        {
            ID = role.ID,
            Name = role.Name,
            Description = role.Description,
            Active = role.Active,
            Permissions = role.RolePermissions
                .Select(rp => new PermissionDto
                {
                    ID = rp.Permission.ID,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description,
                    Module = rp.Permission.Module,
                    Active = rp.Permission.Active
                })
                .ToList()
        };
    }
}
