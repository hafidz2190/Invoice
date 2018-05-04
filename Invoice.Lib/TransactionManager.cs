using Invoice.DataAccess;
using Invoice.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Invoice.Lib
{
    public class TransactionManager
    {
        private DataManager _dataManager;

        public TransactionManager()
        {
            _dataManager = new DataManager();
        }

        public IList<TransactionModel> RetrieveTransactions()
        {
            IList<TransactionModel> result;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    result = DoRetrieveTransactions(session);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return result;
        }

        public TransactionModel CreateTransaction(InvoiceModel invoiceModel)
        {
            TransactionModel result;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        result = DoCreateTransaction(session, invoiceModel);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }

            return result;
        }

        public IList<TransactionModel> DoRetrieveTransactions(ISession session)
        {
            return _dataManager.RetrieveData<TransactionModel>(session);
        }

        public TransactionModel DoCreateTransaction(ISession session, InvoiceModel invoiceModel)
        {
            TransactionModel newTransactionModel = new TransactionModel()
            {
                CreatedTime = DateTime.Now,
                ModifiedTime = DateTime.Now,
                Username = invoiceModel.Username
            };

            float? total = ExtractTotalValue(invoiceModel.Content);
            newTransactionModel.InvoiceContent = invoiceModel.Content;
            newTransactionModel.Total = total;

            Guid newId = (Guid)_dataManager.CreateData(session, newTransactionModel);

            newTransactionModel.Id = newId;

            return newTransactionModel;
        }

        private float? ExtractTotalValue(string content)
        {
            float? total = DoExtractTotalValue(content);

            return total;
        }

        private float? DoExtractTotalValue(string content)
        {
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                string line = lines[i];
                line = line.ToLower();

                if (!line.Contains("total"))
                    continue;

                string[] parts = line.Split(new string[] { "total" }, StringSplitOptions.None);

                for (int j = parts.Length - 1; j >= 0; j--)
                {
                    string part = parts[j];

                    string[] numerics = part.Split(new string[] { " " }, StringSplitOptions.None);

                    for (int k = numerics.Length - 1; k >= 0; k--)
                    {
                        string numeric = numerics[k];
                        float floatNumeric;

                        try
                        {
                            floatNumeric = float.Parse(numeric, NumberStyles.Any, new CultureInfo("id-ID"));
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                        return floatNumeric;
                    }
                }
            }

            return null;
        }
    }
}
