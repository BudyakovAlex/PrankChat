﻿using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionDataModel Map(this TransactionApiModel transactionApiModel)
        {
            if (transactionApiModel is null)
            {
                return null;
            }

            return new TransactionDataModel(transactionApiModel.Id,
                                            transactionApiModel.Amount,
                                            transactionApiModel.Comment,
                                            transactionApiModel.Direction,
                                            transactionApiModel.Reason,
                                            transactionApiModel.BalanceBefore,
                                            transactionApiModel.BalanceAfter,
                                            transactionApiModel.FrozenBefore,
                                            transactionApiModel.FrozenAfter,
                                            transactionApiModel.User?.Map());
        }

        public static TransactionDataModel Map(this DataApiModel<TransactionApiModel> dataApiModel)
        {
            if (dataApiModel.Data is null)
            {
                return null;
            }

            return new TransactionDataModel(dataApiModel.Data.Id,
                                            dataApiModel.Data.Amount,
                                            dataApiModel.Data.Comment,
                                            dataApiModel.Data.Direction,
                                            dataApiModel.Data.Reason,
                                            dataApiModel.Data.BalanceBefore,
                                            dataApiModel.Data.BalanceAfter,
                                            dataApiModel.Data.FrozenBefore,
                                            dataApiModel.Data.FrozenAfter,
                                            dataApiModel.Data.User?.Map());
        }
    }
}