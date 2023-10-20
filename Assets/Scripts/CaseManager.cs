using TMPro;
using UnityEngine;

public class CaseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CasePrefab;
    [SerializeField]
    private TMP_Text casesText;

    private int _case;
    void Start()
    {
        Events.CaseChange += CreatedCase;
        Events.UpdateCase += UpdateCases;
        _case = PlayerPrefs.HasKey("Case") ? PlayerPrefs.GetInt("Case") : 0;
        casesText.text = _case.ToString();
    }
    private void OnDestroy()
    {
        Events.CaseChange -= CreatedCase;
        Events.UpdateCase -= UpdateCases;
    }
    void Update()
    {
        
    }
    void CreatedCase(Vector3 Pos)
    {
        var tempDiamond = Instantiate(CasePrefab);
        Vector3 tempPos = Pos;
        tempPos.y = CasePrefab.transform.position.y;
        tempDiamond.transform.position = tempPos;
    }
    public void UpdateCases()
    {
        _case++;
        PlayerPrefs.SetInt("Case", _case);
        casesText.text = _case.ToString();
    }

}
