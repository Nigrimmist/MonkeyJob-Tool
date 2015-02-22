MonkeyJob Tool
========

.NET/C# Personal program assistant for improving productivity. Do it faster. Make it better.

!["MonkeyJob Tool screenshot"](http://c2n.me/3cRZZgb.png "MonkeyJob Tool work sample")

#Description

MonkeyJob Tool is a program, that allow you to do any kind of typical things by simply typing short commands.

###Samples of usage 1
For example, if you need to find any place in google maps, in common you should :

**put your hands to the mouse, click to the browser, enter maps.google.com, enter your address, for example "221 Baker St London" and waiting for response.** - it will take about... 30-40 seconds.

And let's **compare it** with same task, but using MonkeyJob Tool :

**press "CTRL+M" (configurable hotkey) -> enter "map 221 Baker St London" -> press "enter". PROFIT!** - tool will open google maps in your default browser and it will take 5-10 seconds. Cool, right?


###Samples of usage 2

You need to translate "Barak Obama" to russian language. Using MonkeyJob tool you should only press hotkey for open "console" and type "t Barak Obama" and "Translate" module will show you required translation to Russian. (Currently "Translate" module support only two languages (Russian/English), but it can be and it will be extended to others).

###Samples of usage N
MonkeyJob Tool can be extended by external "modules" for any kind answers and actions. It mean, that you can can write your own modules using our documentation behind.

###Base commands
You can type "modules" to see all included modules with description notes.

#Getting started
####Downloads
You can download stand-alone version (install non-required) using "Releases" link in top of current page. 

####Adding modules and commands


For adding your own command handlers you should implement two interfaces in your dll : **IActionHandler** and **IActionHandlerRegister**. They are placed in HelloBotCommunication.dll (HelloBotCommunication project).

**1.**  Implementing IActionHandler you should define CallCommandList, CommandDescription properties and HandleMessage method. CallCommandList should return List of pre-defined command info objects. Each defined command name will fire your HandleMessage method.

Handled message (your answer) should be returned using sendMessageFunc callback. 

**2.** Implementing IActionHandlerRegister you should define list of your IActionHandler modules.

- **Sample of Calculator command**
```C#
//Calculate command handler


public class Calculator : IActionHandler
    {

        public List<CallCommandInfo> CallCommandList
        {
            get
            {
                return new List<CallCommandInfo>()
                {
                    new CallCommandInfo("calc"),
                    new CallCommandInfo("калькулятор")
                };
            }
        }
        public string CommandDescription { get { return "Clever calculator using NCalc library"; }  }
        
        //that method will be fired when anybody will call your command from client
        public void HandleMessage(string args, object clientData, Action<string, AnswerBehaviourType> sendMessageFunc)
        {
            
            //calculate expression passing arguments to Expression NCalc's class constructor.
            //args can be, for example, "1+2/3". You should check it for valid format in context of you command.
            Expression expr = new Expression(args);
            var exprAnswer = expr.Evaluate();
            string messageAuthor = string.Empty;
            string answer = string.Empty;

            answer = string.Format("Ответ равен : {0}", exprAnswer);
            //send answer back to client
            sendMessageFunc(answer, AnswerBehaviourType.Text);
            
        }
    }
```

And another one class for register your calculator handler :

```C#
//That class will register your modules
   public class DllRegister : IActionHandlerRegister
   {
       public List<IActionHandler> GetHandlers()
       {
           return new List<IActionHandler>()
           {
               new Calculator(),
               //and others if exist
           };
       }
   }
```

After that you can simply put your dll to bin folder of client application and it should work.



###Supported system commands
- "help" - show list of system commands
- "modules" - show custom module list
