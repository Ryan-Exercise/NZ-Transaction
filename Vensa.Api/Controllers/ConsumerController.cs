//  ConsumerController.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       10:17:7 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Vensa.Api.Exceptions;
using Vensa.Api.Services;

namespace Vensa.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ConsumerController : ControllerBase
    {
        private readonly IConsumerService _consumerService;
        private readonly ILogger<IConsumerService> _logger;

        public ConsumerController(
            IConsumerService consumerService,
            ILogger<IConsumerService> logger)
        {
            _consumerService = consumerService;
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult SearchConsumerByname(
            [Required]string firstName, [Required]string lastName, [Required]string dateOfBirth)
        {
            try
            {
                string first = Normalize(firstName);
                string last = Normalize(lastName);
                string birth = Normalize(dateOfBirth);
                if(string.IsNullOrEmpty(first)
                    || string.IsNullOrEmpty(last)
                    || string.IsNullOrEmpty(dateOfBirth))
                {
                    return NotFound($"{first} {last} born on {birth} is not found");
                }
                
                var consumer = _consumerService.GetConsumerWithBalance(
                    Normalize(firstName), Normalize(lastName), Normalize(dateOfBirth));
                return Ok(consumer);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
                
            }
            catch(MultipleEntitiesFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound("Uknown error, please check your input");
            }
        }

        [HttpGet]
        [HttpPost]
        public IActionResult GetConsumerTransactionsByConsumerId(
            [Required]int consumerId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if(consumerId > 0)
                {
                    var transactions = _consumerService.GetConsumerTransactions(
                    consumerId, pageNumber, pageSize);
                    return Ok(new
                    {
                        consumerId,
                        transactions.PageNumber,
                        transactions.TotalPages,
                        transactions.TotalItems,
                        transactions = transactions.PageData
                    });
                }
                return NotFound("Invalid consumer's Id");
            }
            catch(EntityNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(StatusCodes.Status500InternalServerError);
            }
        }

        private string Normalize(string value)
        {
            return value?.Replace(" ","").ToLower();
        }
    }
}
