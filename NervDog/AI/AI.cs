using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using NervDog.Common;
using NervDog.Soul;

namespace NervDog.AI
{
    public class AI : EventListener
    {
        private readonly string _AIScriptPath;
        private readonly ScriptRuntime _PyRuntime;

        private readonly Guid _id;
        private dynamic _AIObject;
        private Character _character;

        public AI(string AIScriptPath, Character character)
            : base(Constants.AI_EVENT)
        {
            _character = character;
            _AIScriptPath = AIScriptPath;
            _id = Guid.NewGuid();

            //Load AIScript
            _PyRuntime = Python.CreateRuntime();
            LoadAIScript(AIScriptPath);
        }

        public Guid ID
        {
            get { return _id; }
        }

        public string AIScriptPath
        {
            get { return _AIScriptPath; }
        }

        public Character Character
        {
            set
            {
                if (_character == null)
                {
                    _character = value;
                }
            }
            get { return _character; }
        }

        //Override
        public override EventFunction Handler
        {
            set { _handler = value; }
            get { return e => Execute(); }
        }

        public void LoadAIScript(string AIScriptPath)
        {
            _AIObject = _PyRuntime.UseFile(AIScriptPath);
            _AIObject.THIS = _character;
        }

        public void Execute()
        {
            _AIObject.Execute();
        }
    }
}