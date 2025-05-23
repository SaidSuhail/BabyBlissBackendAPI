﻿using AutoMapper;
using BabyBlissBackendAPI.Data;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyBlissBackendAPI.Services.AddressServices
{
    public class AddressServices:IAddressServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AddressServices(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool>AddnewAddress(int userId,AddNewAddressDto address)
        {
            try
            {
                if(address == null)
                {
                    throw new ArgumentNullException(nameof(address),"Address information is required.");
                }
                var user = await _context.users.FindAsync(userId);
                if(user == null)
                {
                    throw new Exception("User not found");
                }
                var newAdd = _mapper.Map<UserAddress>(address);
                newAdd.UserId = userId;
                _context.userAddress.Add(newAdd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while saving changes:{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<List<GetAddressDto>>GetAddress(int userId)
        {
            try
            {
                var user = await _context.users.FindAsync(userId);
                if(user == null)
                {
                    throw new Exception("User not found");
                }
                var address = await _context.userAddress.Where(a => a.UserId == userId).ToListAsync();
                return _mapper.Map<List<GetAddressDto>>(address);
            }
            catch(Exception ex)
            {
                throw new Exception($"An error occured while saving changes:{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<bool>RemoveAddress(int addId)
        {
            try
            {
                var adres = await _context.userAddress.FirstOrDefaultAsync(a => a.Id == addId);
                if(adres == null)
                {
                    return false;
                }
                _context.userAddress.Remove(adres);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }
    }
}
