﻿using AutoMapper;
using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Customers.Registration
{
    public class CustomerRegistrationMapper:Profile
    {
        /// <summary>
        /// Mapper from UserRegistrationRequest to User
        /// Doesnt map password because password shoulb be hashed
        /// </summary>
        public CustomerRegistrationMapper()
        {
            CreateMap<CustomerRegistrationRequest, User>()
                    .ForMember(user => user.FirstName,
                               ops => ops.MapFrom(request => request.FirstName))
                    .ForMember(user => user.LastName,
                               ops => ops.MapFrom(request => request.LastName))
                    .ForMember(user => user.Email,
                               ops => ops.MapFrom(request => request.Email))
                    .ForMember(user => user.UserName,
                               ops => ops.MapFrom(request => request.Email))
                    .ForMember(user => user.PhoneNumber,
                               ops => ops.MapFrom(request => request.PhoneNumber));


            CreateMap<Customer, CustomerRegistrationResponse>()
                    .ForMember(user => user.FirstName,
                               ops => ops.MapFrom(request => request.UserInfo.FirstName))
                    .ForMember(user => user.LastName,
                               ops => ops.MapFrom(request => request.UserInfo.LastName))
                    .ForMember(user => user.Email,
                               ops => ops.MapFrom(request => request.UserInfo.Email))
                    .ForMember(user => user.Id,
                               ops => ops.MapFrom(request => request.Id))
                    .ForMember(user => user.RoleName,
                               ops => ops.MapFrom(request => request.Role.Name));

        }

    }
}
