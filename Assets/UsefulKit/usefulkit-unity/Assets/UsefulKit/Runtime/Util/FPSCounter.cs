/* 
* Adapted from https://forum.unity.com/threads/fpstotext-free-fps-framerate-calculator-with-options.463667/
*/

using System.Globalization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSCounter : MonoBehaviour
{
    [Header("Sample Groups of Data ")]
    public bool groupSampling = true;
    public int sampleSize = 20;

    [Header("Config ")]
    public int updateTextEvery = 1;
    public int maxTextLength = 5;
    public bool smoothed = true;
    public bool forceIntResult = true;
    public bool zeroAllocOnly = false;

    [Header("System FPS (updates once/sec)")]
    public bool useSystemTick = false;

    [Header("Color Config ")]
    public bool useColors = true;
    public Color good = Color.green;
    public Color okay = Color.yellow;
    public Color bad = Color.red;
    public int okayBelow = 60;
    public int badBelow = 30;

    public float framerate { get { return fps; } }

    protected float[] fpsSamples;
    protected int sampleIndex;
    protected int textUpdateIndex;

    private TextMeshProUGUI tmp;
    private float fps;
    private int sysLastSysTick;
    private int sysLastFrameRate;
    private int sysFrameRate;
    private string localfps;

    private static readonly string[] fpsStringMap = {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99",
            "100", "101", "102", "103", "104", "105", "106", "107", "108", "109",
            "110", "111", "122", "113", "114", "115", "116", "117", "118", "119",
            "120", "121", "132", "123", "124", "125", "126", "127", "128", "129",
            "130", "131", "142", "133", "134", "135", "136", "137", "138", "139",
            "140", "141", "152", "143", "144", "145", "146", "147", "148", "149",
            "150", "151", "162", "153", "154", "155", "156", "157", "158", "159",
            "160", "161", "172", "163", "164", "165", "166", "167", "168", "169",
            "170", "171", "182", "173", "174", "175", "176", "177", "178", "179",
            "180", "181", "192", "183", "184", "185", "186", "187", "188", "189",
            "190", "191", "192", "193", "194", "195", "196", "197", "198", "199",
            "200", "201", "202", "203", "204", "205", "206", "207", "208", "209",
            "210", "211", "222", "213", "214", "215", "216", "217", "218", "219",
            "220", "221", "222", "223", "224", "225", "226", "227", "228", "229",
            "230", "231", "232", "233", "234", "235", "236", "237", "238", "239",
            "240", "241", "242", "243", "244", "245", "246", "247", "248", "249",
            "250", "251", "252", "253", "254", "255", "256", "257", "258", "259",
            "260", "261", "262", "263", "264", "265", "266", "267", "268", "269",
            "270", "271", "272", "273", "274", "275", "276", "277", "278", "279",
            "280", "281", "282", "283", "284", "285", "286", "287", "288", "289",
            "290", "291", "292", "293", "294", "295", "296", "297", "298", "299+"
        };

    protected virtual void Reset()
    {
        sampleSize = 20;
        updateTextEvery = 1;
        maxTextLength = 5;
        smoothed = true;
        useColors = true;
        good = Color.green;
        okay = Color.yellow;
        bad = Color.red;
        okayBelow = 60;
        badBelow = 30;
        useSystemTick = false;
        forceIntResult = true;
    }

    protected virtual void Start()
    {
        fpsSamples = new float[sampleSize];
        for (int i = 0; i < fpsSamples.Length; i++) fpsSamples[i] = 0.001f;
        tmp = GetComponent<TextMeshProUGUI>();
        if (!tmp) enabled = false;
    }

    protected virtual void Update()
    {
        if (groupSampling) Group();
        else SingleFrame();

        localfps = zeroAllocOnly ? fpsStringMap[Mathf.Clamp((int)fps, 0, 299)] : fps.ToString(CultureInfo.CurrentCulture);

        sampleIndex = sampleIndex < sampleSize - 1 ? sampleIndex + 1 : 0;
        textUpdateIndex = textUpdateIndex > updateTextEvery ? 0 : textUpdateIndex + 1;
        if (textUpdateIndex == updateTextEvery) tmp.text = localfps.Substring(0, localfps.Length < 5 ? localfps.Length : 5);

        if (!useColors) return;
        if (fps < badBelow)
        {
            tmp.color = bad;
            return;
        }
        tmp.color = fps < okayBelow ? okay : good;
    }

    protected virtual void SingleFrame()
    {
        fps = useSystemTick
            ? GetSystemFramerate()
            : smoothed ? 1 / Time.smoothDeltaTime : 1 / Time.deltaTime;
        if (forceIntResult) fps = (int)fps;
    }

    protected virtual void Group()
    {
        fpsSamples[sampleIndex] = useSystemTick
            ? GetSystemFramerate()
            : smoothed ? 1 / Time.smoothDeltaTime : 1 / Time.deltaTime;

        fps = 0;
        bool loop = true;
        int i = 0;
        while (loop)
        {
            if (i == sampleSize - 1) loop = false;
            fps += fpsSamples[i];
            i++;
        }
        fps /= fpsSamples.Length;
        if (forceIntResult) fps = (int)fps;
    }

    protected virtual int GetSystemFramerate()
    {
        if (System.Environment.TickCount - sysLastSysTick >= 1000)
        {
            sysLastFrameRate = sysFrameRate;
            sysFrameRate = 0;
            sysLastSysTick = System.Environment.TickCount;
        }
        sysFrameRate++;
        return sysLastFrameRate;
    }
}
