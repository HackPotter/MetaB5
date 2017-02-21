using System.Collections.Generic;
using UnityEngine;

public class NucleotideChain : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject _chainRoot;
#pragma warning restore 0067, 0649

    private List<GameObject> _chain = new List<GameObject>();
    private Dictionary<GameObject, GameObject[]> _nucleotideBasesByChainLink = new Dictionary<GameObject, GameObject[]>();
    private List<GameObject> _activeNucleotides = new List<GameObject>();
    private int _firstDisabledLinkIndex = 0;

    public enum NucleotideType
    {
        Adenine = 0,
        Cytosine = 1,
        Guanine = 2,
        Thymine = 3,
        Uracil = 4,
    }

    void Awake()
    {
        Transform currentLink = _chainRoot.transform;
        while ((currentLink = currentLink.transform.Find("Nucleotide")) != null)
        {
            _chain.Add(currentLink.gameObject);
            _nucleotideBasesByChainLink.Add(currentLink.gameObject, new GameObject[5]);
            _nucleotideBasesByChainLink[currentLink.gameObject][0] = currentLink.Find("A").gameObject;
            _nucleotideBasesByChainLink[currentLink.gameObject][1] = currentLink.Find("C").gameObject;
            _nucleotideBasesByChainLink[currentLink.gameObject][2] = currentLink.Find("G").gameObject;
            _nucleotideBasesByChainLink[currentLink.gameObject][3] = currentLink.Find("T").gameObject;
            _nucleotideBasesByChainLink[currentLink.gameObject][4] = currentLink.Find("U").gameObject;
        }
        ResetAll();
    }

    public void ResetChain()
    {
        foreach (var link in _chain)
        {
            link.SetActive(false);
        }
    }

    public void UnlockNext(string nucleotideType)
    {
        switch (nucleotideType)
        {
            case "A":
                UnlockNext(NucleotideType.Adenine);
                break;
            case "C":
                UnlockNext(NucleotideType.Cytosine);
                break;
            case "G":
                UnlockNext(NucleotideType.Guanine);
                break;
            case "T":
                UnlockNext(NucleotideType.Thymine);
                break;
            case "U":
                UnlockNext(NucleotideType.Uracil);
                break;
        }
    }

    public void UnlockNext(NucleotideType nucleotideType)
    {
        if (_firstDisabledLinkIndex >= _chain.Count)
        {
            return;
        }
        GameObject chainLink = _chain[_firstDisabledLinkIndex];
        chainLink.SetActive(true);
        GameObject nucleotide = _nucleotideBasesByChainLink[_chain[_firstDisabledLinkIndex]][(int)nucleotideType];
        nucleotide.SetActive(true);
        _activeNucleotides.Add(nucleotide);
        _firstDisabledLinkIndex++;

    }

    public void ResetAll()
    {
        _firstDisabledLinkIndex = 0;
        foreach (var go in _activeNucleotides)
        {
            go.SetActive(false);
        }
        _activeNucleotides.Clear();
        ResetChain();
    }
}
