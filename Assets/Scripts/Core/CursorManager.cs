using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CursorManager : IInitializable, IDisposable
{

    private readonly IConsole _console;
    private readonly Dictionary<object, RequestInfo> _requests = new Dictionary<object, RequestInfo>();

    public CursorManager(IConsole console)
    {
        _console = console;
    }

    public void Initialize()
    {
        _console.RegisterObject(this);
        UpdateCursor();
    }

    public void Dispose()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Show(object requestingObject)
    {
        _requests.Add(requestingObject, new RequestInfo(DateTime.Now));
        UpdateCursor();
    }

    public void Hide(object requestingObject)
    {
        _requests.Remove(requestingObject);
        UpdateCursor();
    }

    [ConsoleCommand("Dick")]
    public void PrintCursorRequests()
    {
        _console.Log($"Active cursor requests ({_requests.Count}):");
        foreach (var request in _requests)
        {
            _console.Log($"{request.Key}. Requested: {request.Value.RequestionTime}");
        }
    }

    private void UpdateCursor()
    {
        Cursor.lockState = _requests.Count > 0 ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _requests.Count > 0;
    }

    private struct RequestInfo
    {

        public readonly DateTime RequestionTime;

        public RequestInfo(DateTime requestionTime)
        {
            RequestionTime = requestionTime;
        }

    }

}