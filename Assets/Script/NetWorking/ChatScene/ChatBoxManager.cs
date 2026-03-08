using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxManager : MonoBehaviour
{
    public static ChatBoxManager Instance;
    public event EventHandler<ChatBoxMessage> OnMessageSend;

    [SerializeField] private TMP_Text _txtLabel;
    [SerializeField] private TMP_Dropdown _dropdownTarget;
    [SerializeField] private TMP_Dropdown _dropdownColors;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _bpSend;
    [SerializeField] private Transform _textHolder;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private TMP_Text _prfText;

    [SerializeField] private Sprite _spriteDropdownColor;
    [SerializeField] private Color[] _colors = {
        Color.black, Color.red, Color.blue, Color.green
    };

    private string _currentPlayerName;
    private int _currentSelectionColor = 0;
    private string _currentTarget ="All";
    private string[] _playerNames;

    private void Awake() {
        Instance = this;
        _bpSend.onClick.AddListener(UISendMessage);
        _inputField.onSubmit.AddListener(SubmitTestFromInputField);
        _dropdownTarget.onValueChanged.AddListener( SetCurrentTarget);
        _dropdownColors.onValueChanged.AddListener(SetCurrentColor);
        SetUpDropdownColors();
        gameObject.SetActive(false);
    }
    
    public void DisplayMessage(ChatBoxMessage message) {
        string messageText = String.Empty;
        if (message.PlayerTarget != "All") {
            messageText = "[privé]";
            if(_currentPlayerName!= message.PlayerTarget)return;
        }
        TMP_Text text = Instantiate(_prfText, _textHolder);
        text.text = messageText + message.PlayerFrom + ":" + message.Text;
        text.color = GetColorById(message.TextColor);
        
        Invoke("SetTextToBot", 0.1f);
    }
    public void SetupDropdownTargets(stringContainer[] targets) {
        List<string > targetNames = new List<string>();
        
        _dropdownTarget.ClearOptions();
        List<TMP_Dropdown.OptionData>options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData("All"));
        foreach (var name in targets) {
            options.Add(new TMP_Dropdown.OptionData(name.SomeText));
            targetNames.Add(name.SomeText);
        }
        _playerNames = targetNames.ToArray();
        _dropdownTarget.AddOptions(options);

        if (_playerNames.Contains(_currentTarget)) {
            _dropdownTarget.SetValueWithoutNotify(_playerNames.ToList().IndexOf(_currentTarget)+1);
            SetCurrentTarget(_playerNames.ToList().IndexOf(_currentTarget));
        }
        else
        {
            _dropdownTarget.SetValueWithoutNotify(-1);
            SetCurrentTarget(-1);
        }
    }
    public void SetCurrentUser(string userName) {
        _txtLabel.text = "User : " + userName;
        _currentPlayerName = userName;
    }

    [ContextMenu ("Test Message Display")]
    private void DebugTestMessage()
    {
        ChatBoxMessage message = new ChatBoxMessage( "All",3,"Hello","Popote");
        DisplayMessage(message);
    }
    private void SetUpDropdownColors() {
        _dropdownColors.ClearOptions();
        List<TMP_Dropdown.OptionData>options = new List<TMP_Dropdown.OptionData>();
        foreach (var color in _colors) {
            options.Add(new TMP_Dropdown.OptionData("",_spriteDropdownColor, color)); }
        _dropdownColors.AddOptions(options);
    }
    private void SetCurrentTarget(int index) {
        if (index == -1) {
            _currentTarget = "All";
            return;
        }
        _currentTarget = _playerNames[index-1];
    }
    private void SetCurrentColor(int colorId) {
        _currentSelectionColor = colorId;
    }
    private void SubmitTestFromInputField(string input) {
        UISendMessage();
    }
    private void UISendMessage() {
        if (String.IsNullOrEmpty(_inputField.text)) return;
        ChatBoxMessage message = new ChatBoxMessage(_currentTarget, _currentSelectionColor, _inputField.text, _currentPlayerName);
        _inputField.text = "";
       OnMessageSend?.Invoke(this, message);
    }
    private void SetTextToBot() {
        LayoutRebuilder.ForceRebuildLayoutImmediate((_textHolder as RectTransform));
        _scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    private Color GetColorById(int id) {
        if (_colors.Length > id) return _colors[id];
        return Color.black;
    }
}