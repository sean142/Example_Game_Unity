using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogSystem : MonoBehaviour
{
    [Header("UI組件")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文件")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

    [Header("頭像")]
    public Sprite face01, face02;

    private bool textFinished; //是否完成打字
    private bool canceTyping; //取消打字

    List<string> textList = new List<string>();

    private void Awake()
    {
        GetTextFormFile(textFile);
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    private void OnDisable()
    {
        textFinished = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter)&&index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (textFinished && !canceTyping)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!textFinished&&!canceTyping)
            {
                canceTyping =true;
            }
        }    
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineDate = file.text.Split('\n');

        foreach (var line in lineDate)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        switch (textList[index])
        {
            case"主":
                 faceImage.sprite = face01;
                index++;
                break;
            case "配":
                faceImage.sprite = face02;
                index++;
                break;
        }
        /*
        for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i];

            yield return new WaitForSeconds(textSpeed);
        }
        */
        int letter = 0;
        while (!canceTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        canceTyping = false;
        textFinished = true;
        index++;
    }
}
