using CbarFormsApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Xml;

namespace CbarFormsApp
{
    public partial class Form1 : Form
    {
        string baseUrl = "https://cbar.az/currencies/";
        private RepoContext? _context;
        string sTimeChosen = "";
        string stringXml = ".xml";
        List<Valute> StoreValutes = new List<Valute>();

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this._context = new RepoContext();

            // Uncomment the line below to start fresh with a new database.
            // this.dbContext.Database.EnsureDeleted();
            this._context.Database.EnsureCreated();
            //this._con.Categories.Load();

            //this.categoryBindingSource.DataSource = dbContext.Categories.Local.ToBindingList();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            this._context?.Dispose();
            this._context = null;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateTime = dateTimePicker1.Value;

            sTimeChosen = dateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            sTimeChosen += stringXml;

            var valtypeForTime = _context.ValTypes.Where(n => n.NameForTime == sTimeChosen).FirstOrDefault();

            if (valtypeForTime is null)
            {
                //take from given link and add to the database
                ValType valtypeForDb = TakeInfosFromThirdPartyAPI(sTimeChosen);
                //Add this info to db with efcore
                AddNewValTypeForTimeNameToTheDb(valtypeForDb);
                StoreValutes = valtypeForDb.Valutes;
            }
            else
            {
                StoreValutes = _context.Valutes.Where(n => n.ValTypeId == valtypeForTime.Id).ToList();
            }

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Items.Add("AZN");
            comboBox2.Items.Add("AZN");
            foreach (var item in StoreValutes)
            {
                comboBox1.Items.Add(item.Name);
                comboBox2.Items.Add(item.Name);
            }
        }

        public ValType TakeInfosFromThirdPartyAPI(string sTimeChosen)
        {
            string url = baseUrl + sTimeChosen;

            HttpClient client = new HttpClient();

            var x = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;

            StreamWriter writerOfFile = new StreamWriter("C:\\Users\\terla\\source\\repos\\CbarFormsApp\\CbarFormsApp\\TextFile.txt");

            writerOfFile.Write(x);
            writerOfFile.Close();

            List<Valute> ValutesString = new List<Valute>();

            var valtypeForTime = new ValType()
            {
                Id = Guid.NewGuid(),
                NameForTime = sTimeChosen,
            };

            XmlDocument doc = new XmlDocument();
            doc.Load(string.Concat("C:\\Users\\terla\\source\\repos\\CbarFormsApp\\CbarFormsApp\\TextFile.txt"));

            foreach (XmlNode node in doc.SelectNodes("ValCurs/ValType/Valute"))
            {
                string valueOfObject = node["Value"].InnerText;
                //valueOfObject = valueOfObject.Replace('.', ',');
                double pointForValueOfObject = Convert.ToDouble(valueOfObject);
                ValutesString.Add(new Valute
                {
                    Id = Guid.NewGuid(),
                    Nominal = node["Nominal"].InnerText,
                    Name = node["Name"].InnerText,
                    Value = pointForValueOfObject,
                    ValTypeId = valtypeForTime.Id
                });
            }

            valtypeForTime.Valutes = ValutesString;

            return valtypeForTime;
        }

        public void AddNewValTypeForTimeNameToTheDb(ValType valType)
        {
            _context.ValTypes.Add(valType);
            _context.SaveChanges();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string first = comboBox1.Text;
            string second = comboBox2.Text;
            double answer = 0;

            double firstPart = Convert.ToDouble(textBox1.Text);
            double secondPart = 1;
            //if first is azn
            if (first == "AZN")
            {

            }
            else
            {
                var itemChosenForFirst = StoreValutes.Where(n => n.Name == first).FirstOrDefault();
                firstPart *= (double)itemChosenForFirst.Value;
            }

            //if second is azn
            if (second == "AZN")
            {

            }
            else
            {
                var itemChosenForSecond = StoreValutes.Where(n => n.Name == second).FirstOrDefault();
                secondPart /= (double)itemChosenForSecond.Value;
            }

            //normal calculation
            answer = firstPart * secondPart;
            textBox2.Text = answer.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var allValutes = _context.Valutes.ToList();
            _context.Valutes.RemoveRange(allValutes);
            _context.SaveChanges();

            var allValTypes = _context.ValTypes.ToList();
            _context.ValTypes.RemoveRange(allValTypes);
            _context.SaveChanges();
        }
    }
}