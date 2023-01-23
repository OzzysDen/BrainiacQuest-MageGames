using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextWriter : MonoBehaviour
{
    private class TagData
    {
        public enum TagType
        {
            None,
            Head,
            Body,
            End,
        }

        public int tagHead_StartIndex;
        public int tagHead_EndIndex;

        public int tagEnd_StartIndex;
        public int tagEnd_EndIndex;

        public int tagBody_StartIndex;
        public int tagBody_EndIndex;

        public TagData(int tagHead_StartIndex, int tagHead_EndIndex, int tagEnd_StartIndex, int tagEnd_EndIndex, int tagBody_StartIndex, int tagBody_EndIndex)
        {
            this.tagHead_StartIndex = tagHead_StartIndex;
            this.tagHead_EndIndex = tagHead_EndIndex;
            this.tagEnd_StartIndex = tagEnd_StartIndex;
            this.tagEnd_EndIndex = tagEnd_EndIndex;
            this.tagBody_StartIndex = tagBody_StartIndex;
            this.tagBody_EndIndex = tagBody_EndIndex;
        }

        public TagType Check(int currentIndex)
        {
            if (currentIndex >= tagHead_StartIndex && currentIndex <= tagHead_EndIndex)//38 >= 20 || 38 <= 35
            {
                return TagType.Head;
            }
            else if (currentIndex >= tagEnd_StartIndex && currentIndex <= tagEnd_EndIndex)
            {
                return TagType.End;
            }
            else if (currentIndex >= tagBody_StartIndex && currentIndex <= tagBody_EndIndex)
            {
                return TagType.Body;
            }
            else
            {
                return TagType.None;
            }
        }

    }

    public float time = 10f;

    [TextArea(15, 20)]
    public string mainText = "";
    public bool isAwake = false;
    public bool isLoop = false;
    public bool waitBetweenLoops = false;
    public float betweenLoopsWaitTime = 1f;

    public UnityEvent finishedWritingText;

    private TextMeshProUGUI text;
    private float lastTime = 0f;
    private float timePerCharacter = 0f;
    private Coroutine writer;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        lastTime = 0f;
    }

    private void OnEnable()
    {
        if (isAwake)
        {
            Write();
        }
    }

    private void OnDisable()
    {
        if (writer != null)
        {
            StopCoroutine(writer);
            StopAllCoroutines();
            writer = null;
        }
    }

    public void Write(string text)
    {
        mainText = text;
        Write();
    }

    public void Write()
    {
        text.text = "";
        timePerCharacter = time / mainText.Length;

        if (writer != null)
        {
            StopCoroutine(writer);
            writer = null;
        }

        writer = StartCoroutine(WriterCoroutine());
    }

    /// <summary>
    /// sadece color etiketini arar
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private List<TagData> GetColorTags(string text)
    {
        //<color="yellow">Hellenistic Card</color>!

        List<TagData> list = new List<TagData>();
        int startIndex = text.IndexOf("<color=");
        int endIndex = -1;

        int tagEndStartIndex = -1;

        while (startIndex != -1)
        {
            endIndex = text.IndexOf(">", startIndex);

            if (endIndex != -1)
            {
                tagEndStartIndex = text.IndexOf("</color>", endIndex);

                if (tagEndStartIndex != -1)
                {

                    list.Add(new TagData(startIndex, endIndex, tagEndStartIndex, tagEndStartIndex + "</color>".Length - 1, endIndex + 1, tagEndStartIndex - 1));
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }

            startIndex = text.IndexOf("<color=", startIndex + 1);
        }

        return list;
    }

    /*
     
     You have found:
        A <color="yellow">Hellenistic Card</color>!
     
     */

    private IEnumerator WriterCoroutine()
    {
        if (text == null)
        {
            writer = null;
            yield break;
        }

        do
        {
            List<TagData> colorTagList = GetColorTags(mainText);
            int numberOfTypedCharacter = 0;
            lastTime = timePerCharacter;

            while (numberOfTypedCharacter < mainText.Length)
            {
                lastTime -= Time.deltaTime;

                if (lastTime <= 0)
                {
                    lastTime = timePerCharacter;
                    numberOfTypedCharacter++;

                    for (int i = 0; i < colorTagList.Count; i++)
                    {
                        //Debug.LogError("numberOfTypedCharacter : " + numberOfTypedCharacter + " mainTextLnegth: " + mainText.Length + " TagStart:" + colorTagList[i].tagHead_StartIndex + " TagEnd:" + colorTagList[i].tagHead_EndIndex);
                        if (colorTagList[i].Check(numberOfTypedCharacter) == TagData.TagType.End)
                        {
                            numberOfTypedCharacter = colorTagList[i].tagEnd_EndIndex + 1;
                            colorTagList.RemoveAt(i);
                            break;
                        }
                        else if (colorTagList[i].Check(numberOfTypedCharacter) == TagData.TagType.Head)
                        {
                            numberOfTypedCharacter = colorTagList[i].tagHead_EndIndex + 1;
                            break;
                        }
                    }
                    //Debug.LogError("numberOfTypedCharacter : " + numberOfTypedCharacter + " mainTextLnegth: " + mainText.Length);
                    text.text = mainText.Substring(0, (numberOfTypedCharacter > mainText.Length ? mainText.Length : numberOfTypedCharacter));
                }

                yield return new WaitForEndOfFrame();
            }

            finishedWritingText.Invoke();

            if (isLoop && waitBetweenLoops)
            {
                yield return new WaitForSeconds(betweenLoopsWaitTime);
            }

            yield return new WaitForEndOfFrame();

        } while (isLoop);


        writer = null;
        yield break;
    }


}