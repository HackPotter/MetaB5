using UnityEngine;

public class UnlockNextNucleotide : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private NucleotideChain _nucleotideChain;

    [SerializeField]
    [ExpressionField(typeof(string), "Nucleotide (A,C,G,T,U)")]
    private Expression _nucleotide;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        _nucleotideChain.UnlockNext(context.Evaluate<string>(_nucleotide));
    }
}
