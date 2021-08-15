//  ProviderController.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       19:21:2 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vensa.Api.Dtos;
using Vensa.Api.Repositories;
using Vensa.Api.Services;

namespace Vensa.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private readonly ILogger<IConsumerService> _logger;

        public ProviderController(
            IProviderService providerService,
            ILogger<IConsumerService> logger)
        {
            _providerService = providerService;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult ListConsumers(string orderBy="id", string orderSort="asc", int pageNumber = 1, int pageSize = 100)
        {
            PagedResult<IEnumerable<ListConsumerDto>> data = null;
            try
            {
                string nOrderBy = Normalize(orderBy);
                string nOrderSort = Normalize(orderSort);
                if(string.IsNullOrEmpty(nOrderBy)
                    || string.IsNullOrEmpty(nOrderSort))
                {
                    data = _providerService.GetConsumerList(pageNumber, pageSize);
                }
                else
                {
                    data = _providerService.GetConsumerList(
                        pageNumber, pageSize, nOrderBy, nOrderSort);
                }
                
            }
            catch(Exception ex)
            {
                data = new PagedResult<IEnumerable<ListConsumerDto>>();
                data.PageData = new List<ListConsumerDto>();
                _logger.LogError(ex.Message);
            }

            return Ok(data);
        }

        [HttpGet]
        [HttpPost]
        public IActionResult SearchConsumers(
            string keyword, string field, string orderBy="Id", string orderSort = "asc",
            int pageNumber = 1, int pageSize = 100)
        {
            PagedResult<IEnumerable<ListConsumerDto>> data = null;
            try
            {
                string nKeyword = Normalize(keyword);
                string nField = Normalize(field);
                string nOrderBy = Normalize(orderBy);
                string nOrderSort = Normalize(orderSort);
                if (string.IsNullOrEmpty(nKeyword)
                    || string.IsNullOrEmpty(nField))
                {
                    data = _providerService.GetConsumerList(
                        pageNumber, pageSize,
                        nOrderBy, nOrderSort);
                }
                else
                {
                    data = _providerService.GetConsumerList(
                        nKeyword, nField, pageNumber, pageSize,
                        nOrderBy, nOrderSort);
                }

            }
            catch (Exception ex)
            {
                data = new PagedResult<IEnumerable<ListConsumerDto>>();
                data.PageData = new List<ListConsumerDto>();
                _logger.LogError(ex.Message);
            }

            return Ok(data);
        }

        private string Normalize(string value)
        {
            return value?.Trim().ToLower();
        }
    }
}
