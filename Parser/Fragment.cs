using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class CFragment
{
    public CFragment parent;
    public List<CFragment> children = new List<CFragment>();
    public enum Typ {PLIK, PROCEDURA, KOMENTARZ, KLUCZOWE, INNE};
    public Typ typ;
    public string text;
 
    public CFragment()
	{
	}

    public CFragment(CFragment rodzic)
    {
        parent = rodzic;
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
