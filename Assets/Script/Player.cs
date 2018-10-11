/// <summary>
/// Defines the <see cref="Player" />
/// </summary>

/// <summary>
/// Defines the <see cref="Player" />
/// </summary>
public class Player
{
    /// <summary>
    /// Defines the id
    /// </summary>
    internal int id;

    /// <summary>
    /// Defines the name
    /// </summary>
    internal string name;

    /// <summary>
    /// Defines the tombola
    /// </summary>
    internal int tombola;

    /// <summary>
    /// Defines the cinq1
    /// </summary>
    internal int cinq1;

    /// <summary>
    /// Defines the cinq2
    /// </summary>
    internal int cinq2;

    /// <summary>
    /// Defines the boughtTicket
    /// </summary>
    internal int boughtTicket;

    /// <summary>
    /// Defines the totalWon
    /// </summary>
    internal decimal totalWon;

    /// <summary>
    /// Defines the moneySpent
    /// </summary>
    internal decimal moneySpent = 0m;

    /// <summary>
    /// Defines the credit
    /// </summary>
    internal decimal credit = 0m;

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="_id">The _id<see cref="int"/></param>
    /// <param name="_name">The _name<see cref="string"/></param>
    public Player(int _id, string _name)
    {
        id = _id;
        name = _name;
    }

    /// <summary>
    /// The getID
    /// </summary>
    /// <returns>The <see cref="int"/></returns>
    public int getID()
    {
        return id;
    }

    /// <summary>
    /// The getName
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    public string getName()
    {
        return name;
    }

    /// <summary>
    /// The getTotalWonPrize
    /// </summary>
    /// <returns>The <see cref="decimal"/></returns>
    public decimal getTotalWonPrize()
    {
        return totalWon;
    }

    /// <summary>
    /// The getTombola
    /// </summary>
    /// <returns>The <see cref="int"/></returns>
    public int getTombola()
    {
        return tombola;
    }

    /// <summary>
    /// The getCinq1
    /// </summary>
    /// <returns>The <see cref="int"/></returns>
    public int getCinq1()
    {
        return cinq1;
    }

    /// <summary>
    /// The getCinq2
    /// </summary>
    /// <returns>The <see cref="int"/></returns>
    public int getCinq2()
    {
        return cinq2;
    }

    /// <summary>
    /// The getBoughtTicket
    /// </summary>
    /// <returns>The <see cref="int"/></returns>
    public int getBoughtTicket()
    {
        return boughtTicket;
    }

    /// <summary>
    /// The getMoneySpent
    /// </summary>
    /// <returns>The <see cref="decimal"/></returns>
    public decimal getMoneySpent()
    {
        return moneySpent;
    }

    /// <summary>
    /// The getCredit
    /// </summary>
    /// <returns>The <see cref="decimal"/></returns>
    public decimal getCredit()
    {
        return credit;
    }

    /// <summary>
    /// The setID
    /// </summary>
    /// <param name="i">The i<see cref="int"/></param>
    public void setID(int i)
    {
        id = i;
    }

    /// <summary>
    /// The setName
    /// </summary>
    /// <param name="n">The n<see cref="string"/></param>
    public void setName(string n)
    {
        name = n;
    }

    /// <summary>
    /// The setTombola
    /// </summary>
    /// <param name="t">The t<see cref="int"/></param>
    public void setTombola(int t)
    {
        tombola = t;
    }

    /// <summary>
    /// The setCinq1
    /// </summary>
    /// <param name="c">The c<see cref="int"/></param>
    public void setCinq1(int c)
    {
        cinq1 = c;
    }

    /// <summary>
    /// The setCinq2
    /// </summary>
    /// <param name="c">The c<see cref="int"/></param>
    public void setCinq2(int c)
    {
        cinq2 = c;
    }

    /// <summary>
    /// The increaseTombola
    /// </summary>
    public void increaseTombola()
    {
        tombola++;
    }

    /// <summary>
    /// The increaseCinq1
    /// </summary>
    public void increaseCinq1()
    {
        cinq1++;
    }

    /// <summary>
    /// The increaseCinq2
    /// </summary>
    public void increaseCinq2()
    {
        cinq2++;
    }

    /// <summary>
    /// The decreaseTombola
    /// </summary>
    public void decreaseTombola()
    {
        tombola--;
    }

    /// <summary>
    /// The dereaseCinq1
    /// </summary>
    public void dereaseCinq1()
    {
        cinq1--;
    }

    /// <summary>
    /// The decreaseCinq2
    /// </summary>
    public void decreaseCinq2()
    {
        cinq2--;
    }

    /// <summary>
    /// The setTotalWonPrize
    /// </summary>
    /// <param name="t">The t<see cref="decimal"/></param>
    public void setTotalWonPrize(decimal t)
    {
        totalWon = t;
    }

    /// <summary>
    /// The increaseTotalWonPrize
    /// </summary>
    /// <param name="t">The t<see cref="decimal"/></param>
    public void increaseTotalWonPrize(decimal t)
    {
        totalWon = totalWon + t;
    }

    /// <summary>
    /// The setBoughtTicket
    /// </summary>
    /// <param name="b">The b<see cref="int"/></param>
    public void setBoughtTicket(int b)
    {
        boughtTicket = b;
    }

    /// <summary>
    /// The increaseBoughtTicket
    /// </summary>
    public void increaseBoughtTicket()
    {
        boughtTicket++;
    }

    /// <summary>
    /// The setMoneySpent
    /// </summary>
    /// <param name="m">The m<see cref="decimal"/></param>
    public void setMoneySpent(decimal m)
    {
        moneySpent = m;
    }

    /// <summary>
    /// The increaseMoneySpent
    /// </summary>
    /// <param name="m">The m<see cref="decimal"/></param>
    public void increaseMoneySpent(decimal m)
    {
        moneySpent = moneySpent + m;
    }

    /// <summary>
    /// The setCredit
    /// </summary>
    /// <param name="c">The c<see cref="decimal"/></param>
    public void setCredit(decimal c)
    {
        credit = c;
    }

    /// <summary>
    /// The increaseCredit
    /// </summary>
    /// <param name="c">The c<see cref="decimal"/></param>
    public void increaseCredit(decimal c)
    {
        credit = credit + c;
    }

    /// <summary>
    /// The decreaseCredit
    /// </summary>
    /// <param name="c">The c<see cref="decimal"/></param>
    public void decreaseCredit(decimal c)
    {
        credit = credit - c;
    }
}
