using ApiLoginJdepaz.Core.Domains.Productos;
using ApiLoginJdepaz.Core.Domains.Usuarios;
using ApiLoginJdepaz.Infraestructure.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiLoginJdepaz.Infraestructure.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<TblUsuarios, UsuarioResponse>();
            CreateMap<TblProductos, ProductoResponse>();
            CreateMap<TblProductos, EmailPasswordResetResponse>();
        }
    }
}
