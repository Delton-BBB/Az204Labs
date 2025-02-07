using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Timers;


//https://cosmin-vladutu.medium.com/activity-functions-are-run-at-least-once-16d261f6cb1e
//Problem i found was using activity durable function made multiple orchestration calls, problem identified in the above link

namespace Durable_Function_App
{
    public static class OrchFunction
    {
        private static System.Timers.Timer _timer;

        [Function(nameof(OrchFunction))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(OrchFunction));
            logger.LogInformation("Saying hello.");
            var outList = new List<string>();
            bool isApproved = false;
            string res = "";

            setTimer();
            while (_timer.Enabled)
            {
                logger.LogInformation("Waiting for approval - Current Time: "+ DateTime.Now);
                Thread.Sleep(18000);
                isApproved = true;

                if (isApproved)
                {
                    _timer.Stop();
                    break;
                }
                
            }


            if (!_timer.Enabled)
            {
                res = $"Request has been Approved!.";
                context.SetCustomStatus("Approved");
            }
            else
            {
                res = $"Request has been escalated to Head of Department.";
                context.SetCustomStatus("Escalated");
            }

            //string res = await context.CallActivityAsync<string>(nameof(Escalate), "Head Of Department");
            outList.Add(res);

            return outList;
        }

        public static void setTimer()
        {
            _timer = new System.Timers.Timer(20000);
            _timer.Start();
            _timer.Elapsed += (e,w) =>
            {
                _timer.Stop();
            };
        }

        //[Function(nameof(Approval))]
        //public static string Approval([ActivityTrigger] string name, FunctionContext executionContext)
        //{
        //    return $"Request for {name} is approved!";
        //}

        //[Function(nameof(Escalate))]
        //public static string Escalate([ActivityTrigger] string name, FunctionContext executionContext)
        //{
        //    return $"Request for {name} is escalated to manager!";
        //}

        [Function("OrchFunction_HttpStart")]
        public static async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("OrchFunction_HttpStart");
            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(OrchFunction));

            
            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
