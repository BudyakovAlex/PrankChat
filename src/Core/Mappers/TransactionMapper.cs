using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrankChat.Mobile.Core.Mappers
{
   public static  class TransactionMapper
    {
        public static TransactionDataModel Map(this TransactionApiModel transactionApiModel)
        {
            return new TransactionDataModel(transactionApiModel.Id,
                                            transactionApiModel.Amount,
                                            transactionApiModel.Comment,
                                            transactionApiModel.Direction,
                                            transactionApiModel.Reason,
                                            transactionApiModel.BalanceBefore,
                                            transactionApiModel.BalanceAfter,
                                            transactionApiModel.FrozenBefore,
                                            transactionApiModel.FrozenAfter,
                                            transactionApiModel.User.Map());
        }

        public static TransactionDataModel Map(this DataApiModel<TransactionApiModel> dataApiModel)
        {
            return new TransactionDataModel(dataApiModel.Data.Id,
                                            dataApiModel.Data.Amount,
                                            dataApiModel.Data.Comment,
                                            dataApiModel.Data.Direction,
                                            dataApiModel.Data.Reason,
                                            dataApiModel.Data.BalanceBefore,
                                            dataApiModel.Data.BalanceAfter,
                                            dataApiModel.Data.FrozenBefore,
                                            dataApiModel.Data.FrozenAfter,
                                            dataApiModel.Data.User.Map());
        }
        
    }
}
