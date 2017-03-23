﻿using System;
using System.Collections.Generic;

namespace AntShares.VM
{
    public class InteropService
    {
        private Dictionary<string, Func<ExecutionEngine, bool>> dictionary = new Dictionary<string, Func<ExecutionEngine, bool>>();

        public InteropService()
        {
            Register("System.ScriptEngine.GetScriptContainer", GetScriptContainer);
            Register("System.ScriptEngine.GetExecutingScriptHash", GetExecutingScriptHash);
            Register("System.ScriptEngine.GetCallingScriptHash", GetCallingScriptHash);
            Register("System.ScriptEngine.GetEntryScriptHash", GetEntryScriptHash);
        }

        protected bool Register(string method, Func<ExecutionEngine, bool> handler)
        {
            if (dictionary.ContainsKey(method)) return false;
            dictionary.Add(method, handler);
            return true;
        }

        internal bool Invoke(string method, ExecutionEngine engine)
        {
            if (!dictionary.ContainsKey(method)) return false;
            return dictionary[method](engine);
        }

        private static bool GetScriptContainer(ExecutionEngine engine)
        {
            engine.EvaluationStack.Push(StackItem.FromInterface(engine.ScriptContainer));
            return true;
        }

        private static bool GetExecutingScriptHash(ExecutionEngine engine)
        {
            engine.EvaluationStack.Push(engine.Crypto.Hash160(engine.ExecutingScript));
            return true;
        }

        private static bool GetCallingScriptHash(ExecutionEngine engine)
        {
            engine.EvaluationStack.Push(engine.Crypto.Hash160(engine.CallingScript));
            return true;
        }

        private static bool GetEntryScriptHash(ExecutionEngine engine)
        {
            engine.EvaluationStack.Push(engine.Crypto.Hash160(engine.EntryScript));
            return true;
        }
    }
}
