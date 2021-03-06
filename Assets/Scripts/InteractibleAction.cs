﻿using Assets.Services;
using HoloToolkit;
using UnityEngine;

[RequireComponent(typeof(SoundEffect))]
public class InteractibleAction : MonoBehaviour
{
    [Tooltip("Drag the Tagalong prefab asset you want to display.")]
    public GameObject ObjectToTagAlong;
    public string Text
    {
        get
        {
            return _note.Content;
        }
        set
        {
            _note.Content = value;
        }
    }

    private StickyNote _note;
    private TextMesh _textMesh;
    private BilboardTextFormatterService _bilboardTextParserService;

    void Start()
    {
        _bilboardTextParserService = new BilboardTextFormatterService();
        _note = GetComponent<StickyNote>();
    }

    public void PerformTagAlong()
    {
        if (ObjectToTagAlong == null)
            return;

        var tagAlong = GameObject.FindGameObjectWithTag("TagAlong") ?? CreateTagAlongObject();
        _textMesh = tagAlong.GetComponent<TextMesh>();
        UpdateTagAlongComponent(tagAlong);

        UpdateBilboardText();

        var sound = tagAlong.GetComponent<SoundEffect>();
        if (sound != null)
        {
            sound.PlaySoundEffect();
        }
    }

    public void UpdateBilboardText()
    {
        _textMesh.text = "";

        if (_textMesh == null || _note.Content == null)
            return;

        _textMesh.text = _bilboardTextParserService.Format(input: _note.Content);
    }

    private GameObject CreateTagAlongObject()
    {
        GameObject item = Instantiate(ObjectToTagAlong);

        item.SetActive(true);
        item.AddComponent<Billboard>();
        item.AddComponent<SimpleTagalong>();

        return item;
    }

    private void UpdateTagAlongComponent(GameObject tagAlong)
    {
        var tagAlongComponent = tagAlong.GetComponent<TagAlong>();
        tagAlongComponent.StickyNote = gameObject;
        tagAlongComponent.Interactable = this;
    }
}