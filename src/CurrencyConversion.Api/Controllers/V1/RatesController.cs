using CurrencyConversion.Application.DTOs;
using CurrencyConversion.Application.Interfaces;
using CurrencyConversion.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConversion.Api.Controllers.V1;

/// <summary>
/// Currency rates and conversion (v1).
/// Blocked currencies: TRY, PLN, THB, MXN (HTTP 400).
/// Requires JWT authentication; User or Admin role.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize(Policy = "RequireUser")]
public class RatesController : ControllerBase
{
    private readonly IRatesService _ratesService;
    private readonly IValidator<string> _latestRatesValidator;
    private readonly IValidator<ConvertRequest> _convertValidator;
    private readonly IValidator<HistoricalRatesRequest> _historicalValidator;

    public RatesController(
        IRatesService ratesService,
        LatestRatesRequestValidator latestRatesValidator,
        IValidator<ConvertRequest> convertValidator,
        IValidator<HistoricalRatesRequest> historicalValidator)
    {
        _ratesService = ratesService;
        _latestRatesValidator = latestRatesValidator;
        _convertValidator = convertValidator;
        _historicalValidator = historicalValidator;
    }

    /// <summary>
    /// Get latest exchange rates for a base currency.
    /// </summary>
    /// <param name="baseCurrency">3-letter ISO code (e.g. EUR). TRY, PLN, THB, MXN are blocked.</param>
    [HttpGet("latest")]
    [ProducesResponseType(typeof(LatestRatesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LatestRatesResponse>> GetLatestRates(
        [FromQuery] string baseCurrency = "EUR",
        CancellationToken cancellationToken = default)
    {
        var validation = await _latestRatesValidator.ValidateAsync(baseCurrency, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _ratesService.GetLatestRatesAsync(baseCurrency.Trim().ToUpperInvariant(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Convert an amount from one currency to another.
    /// </summary>
    /// <param name="request">From/to currencies and amount. TRY, PLN, THB, MXN are blocked.</param>
    [HttpPost("convert")]
    [ProducesResponseType(typeof(ConvertResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConvertResponse>> Convert(
        [FromBody] ConvertRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
            return BadRequest("Request body is required.");

        var validation = await _convertValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _ratesService.ConvertAsync(
            request.FromCurrency.Trim().ToUpperInvariant(),
            request.ToCurrency.Trim().ToUpperInvariant(),
            request.Amount,
            cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get historical rates with optional quote currency and pagination.
    /// </summary>
    /// <param name="baseCurrency">Base currency (blocked: TRY, PLN, THB, MXN).</param>
    /// <param name="quoteCurrency">Optional quote currency.</param>
    /// <param name="fromDate">Start date.</param>
    /// <param name="toDate">End date.</param>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Page size (1–100).</param>
    [HttpGet("historical")]
    [ProducesResponseType(typeof(HistoricalRatesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HistoricalRatesResponse>> GetHistoricalRates(
        [FromQuery] string baseCurrency,
        [FromQuery] string? quoteCurrency,
        [FromQuery] DateTime fromDate,
        [FromQuery] DateTime toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var request = new HistoricalRatesRequest(baseCurrency, quoteCurrency, fromDate, toDate, page, pageSize);
        var validation = await _historicalValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

        var result = await _ratesService.GetHistoricalRatesAsync(request, cancellationToken);
        return Ok(result);
    }
}
