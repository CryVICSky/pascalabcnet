﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class ReplaceYieldExprByVarVisitor : BaseChangeVisitor
    {
        private int _Num = 0;

        private ident NewVarName()
        {
            ++_Num;
            return new ident("$yieldExprVar$" + _Num);
        }

        public static ReplaceYieldExprByVarVisitor New
        {
            get { return new ReplaceYieldExprByVarVisitor(); }
        }

        public static void Accept(procedure_definition pd)
        {
            New.ProcessNode(pd);
        }

        public override void visit(yield_node yn)
        {
            var VarIdent = this.NewVarName();
            VarIdent.source_context = yn.ex.source_context;
            var_statement VS = new var_statement(VarIdent, yn.ex) { source_context = yn.ex.source_context };
            ReplaceStatement(yn, SeqStatements(VS, new yield_node(VarIdent, yn.ex.source_context)));
        }
        public override void visit(yield_sequence_node yn)
        {
            var VarIdent = this.NewVarName();
            VarIdent.source_context = yn.ex.source_context;
            var_statement VS = new var_statement(VarIdent, yn.ex) { source_context = yn.ex.source_context };
            ReplaceStatement(yn, SeqStatements(VS, new yield_sequence_node(VarIdent, yn.ex.source_context)));
        }
    }
}