﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using AdaptiveExpressions.Memory;

namespace AdaptiveExpressions.BuiltinFunctions
{
    /// <summary>
    /// Convert ticks to days number.
    /// </summary>
    public class TicksToDays : ExpressionEvaluator
    {
        private const long TicksPerDay = 24 * 60 * 60 * 10000000L;

        public TicksToDays()
            : base(ExpressionType.TicksToDays, Evaluator, ReturnType.Number, FunctionUtils.ValidateUnaryNumber)
        {
        }

        private static (object value, string error) Evaluator(Expression expression, IMemory state, Options options)
        {
            object value = null;
            string error = null;
            IReadOnlyList<object> args;
            (args, error) = FunctionUtils.EvaluateChildren(expression, state, options);
            if (error == null)
            {
                if (args[0].IsInteger())
                {
                    value = Convert.ToDouble(args[0]) / TicksPerDay;
                }
                else
                {
                    error = $"{expression} should contain an integer of ticks";
                }
            }

            return (value, error);
        }
    }
}