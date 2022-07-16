using FiltrDinamico.Core.Extensions;
using FiltrDinamico.Core.Interpreters;
using FiltrDinamico.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FiltrDinamico.Core
{
    public class FiltroDinamico : IFiltroDinamico
    {
        private readonly IFilterInterpreterFactory _factory;

        public FiltroDinamico(IFilterInterpreterFactory factory)
        {
            _factory = factory;
        }

        public Expression<Func<TType, bool>> FromFiltroItemList<TType>(IReadOnlyList<FiltroItem> filtroItems)
        {
            return filtroItems
                .Select(filtroItem => {
                    var interpreter = _factory.Create<TType>(filtroItem);
                    return ResolveNextInterpreter(interpreter, filtroItem);
                })
                .Aggregate((curr, next) => curr.And(next))
                .Interpret();
        }

        private IFilterTypeInterpreter<TType> ResolveNextInterpreter<TType>(IFilterTypeInterpreter<TType> interpreter, FiltroItem filtroItem)
        {
            if (filtroItem.Or != null)
                return interpreter.Or(_factory.Create<TType>(filtroItem.Or));

            if (filtroItem.And != null)
                return interpreter.And(_factory.Create<TType>(filtroItem.And));

            return interpreter;
        }

    }
}
