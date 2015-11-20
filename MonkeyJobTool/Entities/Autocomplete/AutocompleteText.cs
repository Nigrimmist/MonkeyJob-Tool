using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HelloBotCommunication;
using HelloBotCore.Entities;
using CallCommandInfo = HelloBotCore.Entities.CallCommandInfo;

namespace MonkeyJobTool.Entities.Autocomplete
{
    public class AutocompleteText
    {
        private readonly RichTextBox _bindedControl;
        

        List<AutocompleteTextPart> _parts { get; set; }
        private int _currCaretPos = 0;
        private int _currCaretSelectionLength = 0;

        private int _prevCaretPos = 0;
        private int _prevCaretSelectionLength = 0;
        private bool _changedEventEnabled = true;

        public delegate void CommandFocusedDelegate(string commandText);
        public event CommandFocusedDelegate OnCommandFocused;

        public delegate void CommandBluredDelegate();
        public event CommandBluredDelegate OnCommandBlured;

        public delegate CallCommandInfo TryResolveCommandFromStringDelegate(string text);
        private TryResolveCommandFromStringDelegate _tryResolveCommandFromStringFunc;

        public AutocompleteText(RichTextBox bindedControl, TryResolveCommandFromStringDelegate tryResolveCommandFromStringFunc)
        {
            _bindedControl = bindedControl;
            _tryResolveCommandFromStringFunc = tryResolveCommandFromStringFunc;
            _bindedControl.MouseDown += (sender, args) => { changeCaretPos(); };
            _bindedControl.MouseUp += (sender, args) => { changeCaretPos(); };
            _bindedControl.TextChanged += (sender, args) =>
            {
                var focusedPart = changeCaretPos(true);
                if (_changedEventEnabled)
                {
                    textChanged();
                    ClearParts();
                    CorrectParts();
                    TryResolveCommandIfRequired(focusedPart);
                    TryNotifyAboutCommandFocused(focusedPart);
                    TryNotifyAboutCommandBlured(focusedPart);
                    CheckForAvailableArgumentSuggestions(focusedPart);
                    
                    Log();
                    
                }
                
            };
            _bindedControl.KeyUp += (sender, args) => { changeCaretPos(); };
            _parts = new List<AutocompleteTextPart>();
            AddEmptyCommand();
            _lastText = bindedControl.Text;
        }

        private void AddEmptyCommand()
        {
            AppendParts(false,new AutoCompleteCommandPart()
            {
                Index = 0
            });
        }

        private AutocompleteTextPart changeCaretPos(bool fromChangedTextEvent = false)
        {
            AutocompleteTextPart partInFocus = null;
            if ((_currCaretPos != _bindedControl.SelectionStart || fromChangedTextEvent) || _currCaretSelectionLength != _bindedControl.SelectionLength)
            {
                _prevCaretPos = _currCaretPos;
                _prevCaretSelectionLength = _currCaretSelectionLength;
                _currCaretPos = _bindedControl.SelectionStart;
                _currCaretSelectionLength = _bindedControl.SelectionLength;

                var pos = 0;
                foreach (var part in _parts)
                {
                    if (_currCaretPos >= pos && _currCaretPos <= pos + part.Text.Length)
                    {
                        partInFocus = part;
                        break;
                    }
                    pos += part.Text.Length;
                }

                if (partInFocus != null)
                {
                    SetBackColor(partInFocus.BackColor);
                }

                if (!fromChangedTextEvent)
                {
                    TryNotifyAboutCommandFocused(partInFocus);
                    TryNotifyAboutCommandBlured(partInFocus);
                }

                Console.WriteLine(_prevCaretPos + " " + _currCaretPos);
            }

            
            return partInFocus;
        }

        private void SetBackColor(Color color)
        {
            Console.WriteLine("SetBackColor()");
            _bindedControl.SelectionBackColor = color;
        }

        private string _lastText = string.Empty;
        private void textChanged()
        {

            if (_lastText != _bindedControl.Text)
            {
                if (_prevCaretSelectionLength != 0 && _currCaretSelectionLength == 0)
                {
                    //selected and removed

                    bool shouldInsert = _lastText.Length - _prevCaretSelectionLength != _bindedControl.Text.Length;
                    ChangePartOfText(_lastText.Substring(_prevCaretPos, _prevCaretSelectionLength), _prevCaretPos, _prevCaretPos + _prevCaretSelectionLength, true);
                    if (shouldInsert)
                    {
                        //deleted and inserted
                        Console.WriteLine("and inserted : " + _bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos));
                        ChangePartOfText(_bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos), _prevCaretPos, _prevCaretPos, false);
                    }
                }
                else
                {
                    if (_bindedControl.Text.Length > _lastText.Length)
                    {
                        //added new string
                        Console.WriteLine("added new part : " + _bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos));
                        ChangePartOfText(_bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos), _prevCaretPos, _prevCaretPos, false);
                    }
                    else
                    {
                        var correctedPrevCaretPos = _prevCaretPos;
                        if (_prevCaretPos == _currCaretPos) //del btn pressed
                        {
                            correctedPrevCaretPos++;
                        }
                        //text deleted
                        Console.WriteLine("text deleted : " + _lastText.Substring(_currCaretPos, correctedPrevCaretPos - _currCaretPos));
                        ChangePartOfText(_lastText.Substring(_currCaretPos, correctedPrevCaretPos - _currCaretPos), _currCaretPos, correctedPrevCaretPos, true);
                    }
                }
            }
            _lastText = _bindedControl.Text;
            Console.WriteLine("textChanged()");
        }
        private void TryResolveCommandIfRequired(AutocompleteTextPart focusedPart)
        {
            if (IsCommandInFocus(focusedPart))
            {
                Console.WriteLine("_tryResolveCommandFromStringFunc()");
                var command = _tryResolveCommandFromStringFunc(CommandPart.Text);
                CommandPart.Command = command;
            }
        }
        private void TryNotifyAboutCommandFocused(AutocompleteTextPart focusedPart)
        {
            if (IsCommandInFocus(focusedPart))
            {
                if (OnCommandFocused != null)
                    OnCommandFocused(CommandPart.Text);
            }
        }
        private void TryNotifyAboutCommandBlured(AutocompleteTextPart focusedPart)
        {
            if (!IsCommandInFocus(focusedPart))
            {
                if (OnCommandBlured != null)
                    OnCommandBlured();
            }
        }

        private bool IsCommandInFocus(AutocompleteTextPart focusedPart)
        {
            return (focusedPart!=null && focusedPart.Type == AutocompleteTextPartType.Command) || (_currCaretPos <= CommandPart.Text.Length && !string.IsNullOrEmpty(CommandPart.Text));
        }

        private void ChangePartOfText(string val, int fromPos, int toPos, bool deleted)
        {
            
            int currPos = 0;
            AutocompleteTextPart fromAffectedPart = null;
            AutocompleteTextPart toAffectedPart = null;
            for (int i = 0; i < _parts.Count; i++)
            {
                var part = _parts[i];
                if (fromPos >= currPos && fromPos <= currPos + part.Text.Length)
                {
                    //detect border and try to resolve part select
                    if (fromPos == toPos && fromPos == currPos + part.Text.Length &&
                        i != _parts.Count - 1 &&
                        part.Text.EndsWith(" ") && !_parts[i + 1].Text.StartsWith(" "))
                    {
                        fromAffectedPart = _parts[i + 1];
                    }
                    else
                    {
                        fromAffectedPart = part;
                    }

                }
                if (toPos >= currPos && toPos <= currPos + part.Text.Length)
                {
                    //detect border and try to resolve part select
                    if (fromPos == toPos && fromPos == currPos + part.Text.Length &&
                        i != _parts.Count - 1 &&
                        part.Text.EndsWith(" ") && !_parts[i + 1].Text.StartsWith(" "))
                    {
                        toAffectedPart = _parts[i + 1];
                    }
                    else
                    {
                        toAffectedPart = part;
                    }

                    break;
                }

                currPos += part.Text.Length;
            }
            //one part affected
            if (fromAffectedPart.Index == toAffectedPart.Index || !deleted)
            {
                var startPos = GetStartPartPos(fromAffectedPart.Index);
                if (!deleted)
                {
                    fromAffectedPart.Text = fromAffectedPart.Text.Insert(fromPos - startPos, val);
                }
                else
                {
                    fromAffectedPart.Text = fromAffectedPart.Text.Remove(fromPos - startPos, toPos - fromPos);
                }
            }
            else
            {
                //many parts affected   

                AutocompleteTextPart changedPart = fromAffectedPart;
                do
                {
                    var startPos = GetStartPartPos(changedPart.Index);
                    bool fullRemove = startPos + changedPart.Text.Length - 1 < toPos && fromPos <= startPos;
                    if (fullRemove)
                    {
                        toPos -= changedPart.Text.Length;
                        changedPart.Text = "";
                    }
                    else
                    {
                        if (changedPart.Index == fromAffectedPart.Index)
                        {
                            toPos -= (changedPart.Text.Length - (fromPos - startPos));
                            changedPart.Text = changedPart.Text.Substring(0, fromPos - startPos);
                        }
                        else if (changedPart.Index == toAffectedPart.Index)
                        {
                            changedPart.Text = changedPart.Text.Substring(toPos - startPos);
                        }
                        else
                            throw new ApplicationException("alg fault");

                    }
                } while ((changedPart = _parts.SingleOrDefault(x => x.Index == changedPart.Index + 1 && x.Index <= toAffectedPart.Index)) != null);
            }
            
            
        }

        private void CorrectParts()
        {
            if (_parts.Count > 1)
            {
                for (int i = 0; i < _parts.Count; i++)
                {

                    var part = _parts[i];
                    var nextPart = i != _parts.Count - 1 ? _parts[i + 1] : null;
                    part.Index = i;

                    if (part.Type == AutocompleteTextPartType.PartOfText && nextPart != null && nextPart.Type==AutocompleteTextPartType.PartOfText)
                    {
                        part.Text += nextPart.Text;
                        _parts.RemoveAt(i+1);
                    }
                    
                }
            }

            if (CommandPart.Text.EndsWith(" ") && CommandPart.Command!=null)
            {
                CommandPart.Text = CommandPart.Text.TrimEnd(' ');
                AppendParts(true,new AutoCompleteDelimeterPart()
                {
                    Index = _parts.Count,
                    Text = " "
                },new AutocompleteTextPart()
                {
                    Index = _parts.Count,
                    Text = ""
                });
            }

            if (_parts.Last().Type == AutocompleteTextPartType.Suggestion)
            {
                var suggPart = _parts.Last() as AutoCompleteArgumentSuggestPart;
                if (suggPart.Suggest != null)
                {
                    var argToCompare = suggPart.Text.ToLower();
                    var found = suggPart.SuggestCases.FirstOrDefault(x => x.Value.ToLower() == argToCompare);

                    //if new char added but command for new text not found
                    if (found == null  &&
                        suggPart.Suggest.Value.Length <= suggPart.Text.Length //in case of delete 
                        )
                    {
                        var newTextPart = suggPart.Text.Substring(suggPart.Suggest.Value.Length);
                        suggPart.Text = suggPart.Text.Substring(0, suggPart.Suggest.Value.Length);
                        AppendParts(true,new AutocompleteTextPart()
                        {
                            Index = _parts.Count,
                            Text = newTextPart
                        });
                    }
                }

            }

            //var lastPart = _parts.Last();
            //if (lastPart.Type == AutocompleteTextPartType.Suggestion)
            //{
            //    if (lastPart.Text.EndsWith(" ") && (lastPart as AutoCompleteArgumentSuggestPart).Suggest != null)
            //    {
            //        lastPart.Text = CommandPart.Text.TrimEnd(' ');
            //        _parts.Add(new AutocompleteTextPart()
            //        {
            //            Index = _parts.Count,
            //            Text = " "
            //        });
            //    }
            //}
        }

        private void ClearParts()
        {
            if (_parts.Count > 1)
            {
                for (int i = 0; i < _parts.Count; i++)
                {
                    
                    var part = _parts[i];
                    part.Index = i;
                    //skip command part removing
                    if(part.Type==AutocompleteTextPartType.Command) 
                        continue;
                    
                    if (string.IsNullOrEmpty(part.Text))
                    {
                        if (i != 0 && i != _parts.Count - 1)
                        {
                            if (_parts[i - 1].Text.EndsWith(" ") && _parts[i + 1].Text.StartsWith(" "))
                            {
                                continue;
                            }
                        }
                        _parts.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private int GetStartPartPos(int partPos)
        {
            return _parts.Where(x => x.Index < partPos).Sum(x => x.Text.Length);
        }

        public override string ToString()
        {
            return PartsToString(_parts);
        }

        public string PartsToString(List<AutocompleteTextPart> parts)
        {
            return string.Join("", parts.Select(x => x.Text).ToArray());
        }

        public void SetCommand(CallCommandInfo command)
        {
            CommandPart.Text = command.Command;
            CommandPart.Command = command;
        }

        public void Clear()
        {
            _parts.Clear();
            AddEmptyCommand();
        }

        public void RefreshText()
        {
            _changedEventEnabled = false;
            Console.WriteLine("RefreshText");
            _bindedControl.Text = "";//this.ToString();
            
            int pos = 0;
            foreach (AutocompleteTextPart part in _parts)
            {
                SetBackColor(part.BackColor);
                _bindedControl.AppendText(part.Text);
                pos += part.Text.Length;
            }
            _bindedControl.SelectionStart = _bindedControl.TextLength;
            _changedEventEnabled = true;
            
        }

        

        

        public void NotifyAboutAvailableCommandSuggests(List<string> commands)
        {
            //Console.WriteLine("NotifyAboutAvailableCommandSuggests()");
            //if (commands.Count == 1)
            //{
            //    //save command
            //    Command.RealCommand = commands.First();
            //}
            //else if (commands.Count == 0 && !string.IsNullOrEmpty(Command.RealCommand) && Command.Text.EndsWith(" ")) //full command entered before space
            //{
            //    SetCommand(Command.RealCommand, Command.Text);
            //}
            //else
            //{
            //    Command.RealCommand = null;
            //}
        }

        private AutoCompleteCommandPart CommandPart { get { return _parts.First() as AutoCompleteCommandPart; } }

        public void Log()
        {
            foreach (var part in _parts)
            {
                string details = string.Empty;
                if (part.Type == AutocompleteTextPartType.Command)
                {
                    var command = (part as AutoCompleteCommandPart);
                    details += "[" + (command.Command!=null?command.Command.Command:null) + "]";
                }
                if (part.Type == AutocompleteTextPartType.Suggestion)
                {
                    var command = (part as AutoCompleteArgumentSuggestPart);
                    details += "[" + (command.Suggest != null ? command.Suggest.Value : null) + "]";
                }
                Console.WriteLine("[" + part.Text + "] - " + part.Type + " " + details + Environment.NewLine);
            }
        }

        private void CheckForAvailableArgumentSuggestions(AutocompleteTextPart focusedPart)
        {
            if (CommandPart.Command != null)
            {
                if (CommandPart.Command.CommandArgumentSuggestions != null && !IsCommandInFocus(focusedPart))
                {
                    //string args = PartsToString(_parts.Where(x => x.Type != AutocompleteTextPartType.Command && x.Type != AutocompleteTextPartType.Suggestion && x.Type != AutocompleteTextPartType.Delimeter).ToList()).TrimStart();
                    List<string> argsList = new List<string>();

                    StringBuilder partsSb = new StringBuilder();
                    for (int i = 0; i < _parts.Count; i++)
                    {
                        var part = _parts[i];
                        if (part.Type == AutocompleteTextPartType.Command || part.Type == AutocompleteTextPartType.Delimeter) continue;
                        
                        if (part.Type == AutocompleteTextPartType.Suggestion )
                        {
                            if (partsSb.Length > 0)
                            {
                                argsList.Add(partsSb.ToString());
                                partsSb = new StringBuilder();
                            }
                            continue;
                        }
                        partsSb.Append(part.Text);
                        if (i == _parts.Count - 1)
                        {
                            if (partsSb.Length > 0)
                            {
                                argsList.Add(partsSb.ToString());
                            }
                        }
                    }


                    //todo : thread pool required
                    //new Thread(() =>
                    //{
                        
                    //}).Start();
                    try
                    {
                        if (argsList.Any())
                        {
                            for (int i = 0; i < CommandPart.Command.CommandArgumentSuggestions.Count; i++)
                            {
                                for (int j = 0; j < CommandPart.Command.CommandArgumentSuggestions.Count; j++)
                                {
                                    var comSuggestion = CommandPart.Command.CommandArgumentSuggestions[j];
                                    if (comSuggestion.TemplateParseInfo.Count >= argsList.Count)
                                    {
                                        var argSuggestion = comSuggestion.TemplateParseInfo[j];
                                        if (!string.IsNullOrEmpty(argSuggestion.RegexpPart) && Regex.IsMatch(argsList[j], argSuggestion.RegexpPart))
                                        {
                                            List<AutoSuggestItem> suggestions = comSuggestion.Details.Where(x => x.Key == argSuggestion.Key).Select(x => x.GetSuggestionFunc).Single()();
                                            
                                            //todo : check case when text pasted in the middle ?
                                            if (focusedPart.Index == _parts.Last().Index && focusedPart.Type != AutocompleteTextPartType.Suggestion)
                                            {
                                                focusedPart = new AutoCompleteArgumentSuggestPart()
                                                {
                                                    Index = _parts.Count,
                                                    Text = ""
                                                };
                                                AppendParts(false,focusedPart);
                                            }

                                            var suggPart = focusedPart as AutoCompleteArgumentSuggestPart;
                                            if (suggPart != null)
                                            {
                                                var argToCompare = suggPart.Text.ToLower();
                                                var found = suggestions.FirstOrDefault(x => x.Value.ToLower() == argToCompare);
                                                suggPart.Suggest = found;
                                                suggPart.SuggestCases = suggestions;
                                            }
                                           return;
                                        }
                                        else
                                        {
                                            break; //do not scan next
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //if (OnModuleErrorOccured != null)
                        //    OnModuleErrorOccured(ex, command);
                    }
                }
            }
        }

        private void AppendParts(bool prevPartsWasChanged,params AutocompleteTextPart[] parts)
        {
            foreach (var part in parts)
            {
                _parts.Add(part);
            }
            SetBackColor(_parts[parts.Length-1].BackColor);
            if(prevPartsWasChanged)
                RefreshText();
        }
    }

    public class AutocompleteTextPart
    {
        public string Text { get; set; }
        public virtual AutocompleteTextPartType Type { get {return AutocompleteTextPartType.PartOfText;}  }
        public int Index { get; set; }
        public virtual Color BackColor {get {return Color.Transparent;}}

        public AutocompleteTextPart()
        {
            Text = "";
        }
    }
    подумать как легче и менее костыльней отрисовывть текст - в ноутпаде два вариант. сейчас - с частичной
    public class AutoCompleteCommandPart : AutocompleteTextPart
    { ит
        public override AutocompleteTextPartType Type{get { return AutocompleteTextPartType.Command; }}
        public CallCommandInfo Command { get; set; }

        public override Color BackColor
        {
            get { return Color.CadetBlue; }
        }
    }
    public class AutoCompleteDelimeterPart : AutocompleteTextPart
    {
        public override AutocompleteTextPartType Type { get { return AutocompleteTextPartType.Delimeter; } }
    }
    public class AutoCompleteArgumentSuggestPart : AutocompleteTextPart
    {
        public override AutocompleteTextPartType Type { get { return AutocompleteTextPartType.Suggestion; } }
        public AutoSuggestItem Suggest { get; set; }
        public List<AutoSuggestItem> SuggestCases { get; set; }
        public override Color BackColor
        {
            get { return Color.SandyBrown; }
        }
        public AutoCompleteArgumentSuggestPart()
        {
            SuggestCases = new List<AutoSuggestItem>();
        }
    }

    public enum AutocompleteTextPartType
    {
        Suggestion,
        Command,
        PartOfText,
        Delimeter
    }
}
