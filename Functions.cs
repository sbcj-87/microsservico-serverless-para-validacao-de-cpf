using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

public static class Function1
{
    [FunctionName("ValidateCPF")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string cpf = data?.cpf;

        var response = new
        {
            Valid = ValidateCPF(cpf),
            Error = ValidateCPF(cpf) ? null : "Invalid CPF format"
        };

        return new OkObjectResult(response);
    }

    private static bool ValidateCPF(string cpf)
    {
        Regex regex = new Regex(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$");
        return regex.IsMatch(cpf);
    }
}
