using Pharm_api.DTOs;
using Pharm_api.Models;
using Pharm_api.Data;
using Pharm_api.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pharm_api.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IJwtService jwtService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
        }

        public async Task<string> GenerateTokenAsync(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetUsuarioByUsernameAsync(loginDto.Username);
            if (usuario == null) return string.Empty;
            var token = _jwtService.GenerateToken(usuario);
            return token;
        }

        public async Task<bool> ValidateUserAsync(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetUsuarioByUsernameAsync(loginDto.Username);
            return usuario != null;
        }

        public async Task<UsuarioDto> CreateUsuarioAsync(CreateUsuarioDto createDto)
        {
            var usuario = await _usuarioRepository.CreateUsuarioAsync(createDto);
            return new UsuarioDto
            {
                Id = usuario.CodUsuario,
                Email = usuario.Email,
                Username = usuario.Username
            };
        }

        public async Task AsignarSucursalesAsync(string username, List<int> sucursalesIds)
        {
            await _usuarioRepository.AsignarSucursalesAsync(username, sucursalesIds);
        }

        public async Task<List<SucursalDto>> GetSucursalesUsuarioAsync(string username)
        {
            var sucursales = await _usuarioRepository.GetSucursalesUsuarioAsync(username);
            // Mapear UsuarioSucursalDto a SucursalDto si es necesario, o cambiar el tipo en la interfaz
            return sucursales.Select(s => new SucursalDto
            {
                CodSucursal = s.CodSucursal,
                NomSucursal = s.NomSucursal,
                FechaAsignacion = s.FechaAsignacion,
                Activo = s.Activo
            }).ToList();
        }

        public async Task<UsuarioDto?> GetByUsernameAsync(string username)
        {
            var usuario = await _usuarioRepository.GetUsuarioByUsernameAsync(username);
            if (usuario == null) return null;
            return new UsuarioDto
            {
                Id = usuario.CodUsuario,
                Email = usuario.Email,
                Username = usuario.Username
            };
        }

        public async Task<List<UsuarioDto>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
            return usuarios.Select(u => new UsuarioDto
            {
                Id = u.CodUsuario,
                Email = u.Email,
                Username = u.Username
            }).ToList();
        }

        public int? GetUserIdFromToken(HttpContext httpContext)
        {
            return _jwtService.GetUserIdFromToken(httpContext);
        }

        public string? GetUsernameFromToken(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        }

        public string? GetEmailFromToken(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        }
    }
}