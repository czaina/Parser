using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class CFragment
{
    public CFragment parent;
    public List<CFragment> children = new List<CFragment>();
    public enum Typ {PLIK, PROCEDURA, KOMENTARZ, KLUCZOWE, INNE};
    public Typ typ = Typ.INNE;
    public string text;
 
    public CFragment()
	{
	}

    public CFragment(CFragment rodzic)
    {
        parent = rodzic;
    }

    public CFragment(string wartosc)
    {
        string[] result;
        string[] argument = new string[] { "/*", "*/" };
        int counter = 0;

        typ = Typ.PLIK;
        result = wartosc.Split(argument, System.StringSplitOptions.None);
        foreach (string fragment in result)
        {
            if (counter % 2 == 0)
            {
                if (fragment != "") Nowy(fragment).typ = Typ.INNE;
            }
            else
            {
                if (fragment != "") Nowy(fragment).typ = Typ.KOMENTARZ;
            }
            counter++;
        }
    }

    private void RemoveBlockComment(string wartosc)
    {

    }

    private void RemoveComment(string wartosc)
    {

    }

    public CFragment Nowy()
    {
        children.Add(new CFragment(this));
        return children[children.Count-1];
    }

    public CFragment Nowy(string wartosc)
    {
        children.Add(new CFragment(this));
        children[children.Count - 1].text = wartosc;
        return children[children.Count - 1];
    }

    public void Print()
    {

    }

    public void PrintAll(TextBox editBox)
    {
        editBox.Text += text + "\r\n";
        children.ForEach(delegate (CFragment element) 
        {
            element.PrintAll(editBox);
        });
    }
}
