using UnityEngine;

public class ResetNucleotideChain : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private NucleotideChain _nucleotideChain;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        _nucleotideChain.ResetAll();
    }
}

