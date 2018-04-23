using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class imageScraper2 : MonoBehaviour
{
    public GameObject player;
    public GameObject outputObject;
    private  List<GameObject> imageObjs;
    public GameObject UI_imageObject;

    private string url;

    [Header("Input Objects")]
    public InputField searchInputField;
    public Text searchText;


    [Header("Query Options")]
    public string query;
    public int startPosition;
    public bool filterSimilarResults;
    public string matchString;
    List<string> scr;

    public enum SafeSearchFiltering
    {
        /// <summary>
        /// Filter both explicit text and explicit images (a.k.a. Strict Filtering).
        /// </summary>
        Active,
        /// <summary>
        /// Filter explicit images only - default behavior.
        /// </summary>
        Moderate,
        /// <summary>
        /// Do not filter the search results.
        /// </summary>
        Off
    }


    //Use this for initialization
    public void Start()
    {
        imageObjs = new List<GameObject>();
        searchInputField.ActivateInputField();
    }

    #region Search
    public void Search()
    {         
        MakeRequest(searchText.text);
        searchInputField.ActivateInputField();
    }

    public void MakeRequest(string text)
    {
        query=text;

        string requestUrl = string.Format("http://images.google.com/images?" + "q={0}&start={1}&filter={2}&safe={3}",
                    query, startPosition.ToString(),
                    (filterSimilarResults) ? 1.ToString() : 0.ToString(),
                    SafeSearchFiltering.Active);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

        string resultPage = string.Empty;

        using (HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse())
        {
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                using (StreamReader reader =
                         new StreamReader(responseStream))
                {
                    resultPage = reader.ReadToEnd();
                }
            }
        }
        Regex imagesRegex = new Regex(@"<img.*src=*>");

        /* Regex imagesRegex = new Regex(@"(\x3Ca\s+href=/imgres\" +
                        @"x3Fimgurl=)(?<imgurl>http" +
                        @"[^&>]*)([>&]{1})" +
                        @"([^>]*)(>{1})(<img\ssrc\" +
                        @"x3D)(""{0,1})(?<images>/images" +
                        @"[^""\s>]*)([\s])+(width=)" +
                        @"(?<width>[0-9,]*)\s+(height=)" +
                        @"(?<height>[0-9,]*)");
                        */
       scr= new List<string>();

        for (int i = 0; i < 20; i++)
        {
            Group temp = Regex.Match(resultPage, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1];
            matchString = temp.Value;
            scr.Add(matchString);
            Debug.Log(matchString);
            resultPage = resultPage.Substring(temp.Index + temp.Length);    
        }


        //matchString = Regex.Match(resultPage, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
        //foreach (Match ItemMatch in imagesRegex.Matches(resultPage))
        //{
        //  Debug.Log(ItemMatch);
        //}

        Debug.Log(matchString);


        StopCoroutine(GetImages());
        StartCoroutine(GetImages());
        
        // Debug.Log(imagesRegex[0]);
        // Debug.Log(resultPage);

    }


    private List<Texture2D> images;
    IEnumerator GetImages()
    {
        foreach (GameObject g in imageObjs)
            Destroy(g);

        imageObjs = new List<GameObject>();
        images = new List<Texture2D>();
        for (int i = 0; i < scr.Count; i++)
        {
            WWW www = new WWW(scr[i]);
            yield return www;
            GameObject g = Instantiate(UI_imageObject, transform.position + new Vector3(2 * i, 0, 0), Quaternion.identity) as GameObject;
            g.transform.SetParent(transform);
            Texture2D t = new Texture2D(www.texture.width, www.texture.height);
            www.LoadImageIntoTexture(t);
            t.Apply();

            images.Add(t);
            g.GetComponent<RawImage>().texture = t;
            int _i =i;
            g.GetComponent<Button>().onClick.AddListener(() => ImageSelect(_i));
            imageObjs.Add(g);
        }
    }
    #endregion Search

    public void ImageSelect(int i)
    {
        Debug.Log("Image: "+i);
       
        GameObject g=Instantiate(outputObject,player.transform.position +(player.transform.forward*2), player.transform.rotation *Quaternion.AngleAxis(180, Vector3.up)) as GameObject;
        g.GetComponent<Renderer>().material.mainTexture = images[i];


        player.GetComponentInParent<InterfaceSelect>().ExitObjectSelect();
    }
    

    // Update is called once per frame
    void Update()
    {
        //keep input field active
        searchInputField.ActivateInputField();

        //Return for search
        if (Input.GetKeyDown(KeyCode.Return)) Search();
    }
}