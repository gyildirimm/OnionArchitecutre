using Application.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResponse<T> Run<T>(params IResponse<T>[] args)
        {
            foreach (var logic in args)
            {
                if (!logic.IsSuccess)
                {
                    return logic;
                }
            }

            return null;
        }
    }
}
