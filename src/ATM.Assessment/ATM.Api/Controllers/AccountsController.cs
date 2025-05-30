using ATM.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using ATM.ViewModels;

namespace ATM.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IValidator<TransactionRequest> _transactionValidator;
        private readonly IValidator<TransferRequest> _transferValidator;
        
        public AccountsController(IAccountService accountService,
                                  IValidator<TransactionRequest> transactionValidator,
                                  IValidator<TransferRequest> transferValidator)
        {
            _accountService = accountService;
            _transactionValidator = transactionValidator;
            _transferValidator = transferValidator;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await _accountService.GetAccounts();
            return Ok(accounts);
        }

        
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(int accountId)
        {
            var account = await _accountService.GetAccountById(accountId);
            if (account == null) return NotFound();
            return Ok(account);
        }
        
        [HttpGet("type/{accountType}")]
        public async Task<IActionResult> GetAccountByType(string accountType)
        {
            var account = await _accountService.GetAccountByType(accountType);
            if (account == null) return NotFound();
            return Ok(account);
        }
        
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionRequest request)
        {
            var validationResult = await _transactionValidator.ValidateAsync(request);
            if (!validationResult.IsValid) return BadRequest();
            
            var account = await _accountService.DepositAsync(request.AccountId, request.Amount);
            if (account == null) return NotFound("Account not found");
            return Ok(account);
        }
        
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionRequest request)
        {
            var validationResult = await _transactionValidator.ValidateAsync(request);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
            
            var account = await _accountService.WithdrawAsync(request.AccountId, request.Amount);
            if (account == null) return BadRequest("Insufficient funds or account not found");
            return Ok(account);
        }

        // TODO Implement this
        public async Task<IActionResult> Transfer(
            [FromBody]
            TransferRequest request
        )
        {
            
            throw new NotImplementedException();
            
        }

        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var transactions = await _accountService.GetTransactionsForAccount(accountId);
            return Ok(transactions);
        }
    }
    

}
