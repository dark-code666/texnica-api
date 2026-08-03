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

public class PermissionService : IPermissionService
{
    private readonly ErpDbContext _context;

    public PermissionService(ErpDbContext context)
    {
        _context = context;
    }

    public async Task<List<PermissionDto>> GetAllPermissionsAsync()
    {
        var permissions = await _context.Permissions
            .Where(p => p.Active)
            .OrderBy(p => p.Module)
            .ThenBy(p => p.Name)
            .ToListAsync();

        return permissions.Select(MapToDto).ToList();
    }

    public async Task<List<PermissionDto>> GetPermissionsByModuleAsync(string module)
    {
        var permissions = await _context.Permissions
            .Where(p => p.Active && p.Module == module)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return permissions.Select(MapToDto).ToList();
    }

    public async Task<PermissionDto?> GetPermissionByIdAsync(int id)
    {
        var permission = await _context.Permissions
            .FirstOrDefaultAsync(p => p.ID == id && p.Active);

        return permission != null ? MapToDto(permission) : null;
    }

    public async Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto dto)
    {
        var permission = new Permission
        {
            Name = dto.Name,
            Description = dto.Description,
            Module = dto.Module,
            Active = true
        };

        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync();

        return MapToDto(permission);
    }

    public async Task<bool> DeletePermissionAsync(int id)
    {
        var permission = await _context.Permissions.FindAsync(id);
        if (permission == null)
            return false;

        permission.Active = false;
        await _context.SaveChangesAsync();
        return true;
    }

    private static PermissionDto MapToDto(Permission permission)
    {
        return new PermissionDto
        {
            ID = permission.ID,
            Name = permission.Name,
            Description = permission.Description,
            Module = permission.Module,
            Active = permission.Active
        };
    }
}
