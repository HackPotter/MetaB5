

using UnityEngine;

[AddComponentMenu("Modifiers/Morph Animate")]
[ExecuteInEditMode]
public class MegaMorphAnim : MonoBehaviour
{
	public string	SrcChannel = "None";
	public float	Percent = 0.0f;
	MegaMorphChan	channel;

	public string	SrcChannel1 = "None";
	public float	Percent1 = 0.0f;
	MegaMorphChan	channel1;

	public string	SrcChannel2 = "None";
	public float	Percent2 = 0.0f;
	MegaMorphChan	channel2;

	public string	SrcChannel3 = "None";
	public float	Percent3 = 0.0f;
	MegaMorphChan	channel3;

	public string	SrcChannel4 = "None";
	public float	Percent4 = 0.0f;
	MegaMorphChan	channel4;

	public string	SrcChannel5 = "None";
	public float	Percent5 = 0.0f;
	MegaMorphChan	channel5;

	public string	SrcChannel6 = "None";
	public float	Percent6 = 0.0f;
	MegaMorphChan	channel6;

	public string	SrcChannel7 = "None";
	public float	Percent7 = 0.0f;
	MegaMorphChan	channel7;

	public string	SrcChannel8 = "None";
	public float	Percent8 = 0.0f;
	MegaMorphChan	channel8;

	public string	SrcChannel9 = "None";
	public float	Percent9 = 0.0f;
	MegaMorphChan	channel9;

	public string	SrcChannel10 = "None";
	public float	Percent10 = 0.0f;
	MegaMorphChan	channel10;

	public string	SrcChannel11 = "None";
	public float	Percent11 = 0.0f;
	MegaMorphChan	channel11;

	public string	SrcChannel12 = "None";
	public float	Percent12 = 0.0f;
	MegaMorphChan	channel12;

	public string	SrcChannel13 = "None";
	public float	Percent13 = 0.0f;
	MegaMorphChan	channel13;

	public string	SrcChannel14 = "None";
	public float	Percent14 = 0.0f;
	MegaMorphChan	channel14;

	public string	SrcChannel15 = "None";
	public float	Percent15 = 0.0f;
	MegaMorphChan	channel15;

	public string	SrcChannel16 = "None";
	public float	Percent16 = 0.0f;
	MegaMorphChan	channel16;

	public string	SrcChannel17 = "None";
	public float	Percent17 = 0.0f;
	MegaMorphChan	channel17;

	public string	SrcChannel18 = "None";
	public float	Percent18 = 0.0f;
	MegaMorphChan	channel18;

	public string	SrcChannel19 = "None";
	public float	Percent19 = 0.0f;
	MegaMorphChan	channel19;


	public void SetChannel(MegaMorph mr, int index)
	{
		switch ( index )
		{
			case 0: channel = mr.GetChannel(SrcChannel);	break;
			case 1: channel1 = mr.GetChannel(SrcChannel1); break;
			case 2: channel2 = mr.GetChannel(SrcChannel2); break;
			case 3: channel3 = mr.GetChannel(SrcChannel3); break;
			case 4: channel4 = mr.GetChannel(SrcChannel4); break;
			case 5: channel5 = mr.GetChannel(SrcChannel5); break;
			case 6: channel6 = mr.GetChannel(SrcChannel6); break;
			case 7: channel7 = mr.GetChannel(SrcChannel7); break;
			case 8: channel8 = mr.GetChannel(SrcChannel8); break;
			case 9: channel9 = mr.GetChannel(SrcChannel9); break;
			case 10: channel10 = mr.GetChannel(SrcChannel10); break;
			case 11: channel11 = mr.GetChannel(SrcChannel11); break;
			case 12: channel12 = mr.GetChannel(SrcChannel12); break;
			case 13: channel13 = mr.GetChannel(SrcChannel13); break;
			case 14: channel14 = mr.GetChannel(SrcChannel14); break;
			case 15: channel15 = mr.GetChannel(SrcChannel15); break;
			case 16: channel16 = mr.GetChannel(SrcChannel16); break;
			case 17: channel17 = mr.GetChannel(SrcChannel17); break;
			case 18: channel18 = mr.GetChannel(SrcChannel18); break;
			case 19: channel19 = mr.GetChannel(SrcChannel19); break;
		}
	}

	void Start()
	{
		MegaMorph mr = GetComponent<MegaMorph>();

		if ( mr != null )
		{
			channel = mr.GetChannel(SrcChannel);
			channel1 = mr.GetChannel(SrcChannel1);
			channel2 = mr.GetChannel(SrcChannel2);
			channel3 = mr.GetChannel(SrcChannel3);
			channel4 = mr.GetChannel(SrcChannel4);
			channel5 = mr.GetChannel(SrcChannel5);
			channel6 = mr.GetChannel(SrcChannel6);
			channel7 = mr.GetChannel(SrcChannel7);
			channel8 = mr.GetChannel(SrcChannel8);
			channel9 = mr.GetChannel(SrcChannel9);
			channel10 = mr.GetChannel(SrcChannel10);
			channel11 = mr.GetChannel(SrcChannel11);
			channel12 = mr.GetChannel(SrcChannel12);
			channel13 = mr.GetChannel(SrcChannel13);
			channel14 = mr.GetChannel(SrcChannel14);
			channel15 = mr.GetChannel(SrcChannel15);
			channel16 = mr.GetChannel(SrcChannel16);
			channel17 = mr.GetChannel(SrcChannel17);
			channel18 = mr.GetChannel(SrcChannel18);
			channel19 = mr.GetChannel(SrcChannel19);
		}
	}

	void Update()
	{
		if ( channel != null )	channel.Percent = Percent;
		if ( channel1 != null ) channel1.Percent = Percent1;
		if ( channel2 != null ) channel2.Percent = Percent2;
		if ( channel3 != null ) channel3.Percent = Percent3;
		if ( channel4 != null ) channel4.Percent = Percent4;
		if ( channel5 != null ) channel5.Percent = Percent5;
		if ( channel6 != null ) channel6.Percent = Percent6;
		if ( channel7 != null ) channel7.Percent = Percent7;
		if ( channel8 != null ) channel8.Percent = Percent8;
		if ( channel9 != null ) channel9.Percent = Percent9;
		if ( channel10 != null ) channel10.Percent = Percent10;
		if ( channel11 != null ) channel11.Percent = Percent11;
		if ( channel12 != null ) channel12.Percent = Percent12;
		if ( channel13 != null ) channel13.Percent = Percent13;
		if ( channel14 != null ) channel14.Percent = Percent14;
		if ( channel15 != null ) channel15.Percent = Percent15;
		if ( channel16 != null ) channel16.Percent = Percent16;
		if ( channel17 != null ) channel17.Percent = Percent17;
		if ( channel18 != null ) channel18.Percent = Percent18;
		if ( channel19 != null ) channel19.Percent = Percent19;
	}
}
