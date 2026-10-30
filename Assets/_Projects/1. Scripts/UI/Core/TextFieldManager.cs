using System.Collections.Generic;
using UnityEngine;
using Nova;

public static class TextFieldManager
{
    private static readonly List<CoreTextField> _fields = new();
    private static CoreTextField _focused;

    public static CoreTextField Focused => _focused;
    public static bool IsAnyFocused => _focused != null;

    public static void Register(CoreTextField field){
        if(field == null) return;
        if(!_fields.Contains(field)) _fields.Add(field);
    }

    public static void Unregister(CoreTextField field){
        if(field == null) return;
        _fields.Remove(field);
        if(_focused == field) _focused = null;
    }

    public static void RequestFocus(CoreTextField field){
        if(field == null) return;
        if(_focused == field) return;

        _focused?.Defocus();
        _focused = field;
        _focused.Focus();
    }

    public static void DefocusAll(){
        _focused?.Defocus();
        _focused = null;
    }

    public static void ClearAll(string groupId = null){
        foreach(var f in _fields){
            if(f == null) continue;
            if(groupId != null && f.GroupId != groupId) continue;
            f.Clear();
        }
    }

    public static void SubmitGroup(string groupId){
        foreach(var f in _fields){
            if(f == null) continue;
            if(f.GroupId != groupId) continue;
            f.Submit();
        }
    }

    public static bool IsHitAField(UIBlock hitBlock, out CoreTextField field){
        field = null;
        if(hitBlock == null) return false;

        Transform t = hitBlock.transform;
        while(t != null){
            foreach(var f in _fields){
                if(f != null && f.transform == t){ field = f; return true; }
            }
            t = t.parent;
        }
        return false;
    }

    public static void OnGlobalPress(UIBlock hitBlock){
        if(!IsHitAField(hitBlock, out _))
            DefocusAll();
    }
}