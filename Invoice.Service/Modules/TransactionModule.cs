using Invoice.Lib;
using Invoice.Model;
using Nancy;
using Newtonsoft.Json;
using System;

namespace Invoice.Service
{
    public class TransactionModule : NancyModule
    {
        public TransactionModule()
        {
            var transactionManager = new TransactionManager();

            Get["/"] = _ =>
            {
                return "It's working";
            };

            Get["/RetrieveTransactions"] = _ =>
            {
                return transactionManager.RetrieveTransactions();
            };

            Post["/CreateTransactionFromInvoice"] = _ =>
            {
                try
                {
                    var stream = Request.Body;
                    var length = Request.Body.Length;

                    var data = new byte[length];
                    stream.Read(data, 0, (int)length);

                    var body = System.Text.Encoding.Default.GetString(data);

                    var invoiceModel = JsonConvert.DeserializeObject<InvoiceModel>(body);

                    var result = transactionManager.CreateTransaction(invoiceModel);

                    return Response.AsJson(result, HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    return Response.AsJson(e, HttpStatusCode.InternalServerError);
                }
            };
        }
    }
}
