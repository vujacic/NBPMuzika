using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NBPMuzika.Models.Entiteti;
using Neo4jClient;

namespace NBPMuzika.Models.Baze
{
    public class Neo4j
    {
        public Neo4j() { }

        Neo4jClient.Cypher.CypherQuery vratiRezultat(string query)
        {
            return new Neo4jClient.Cypher.CypherQuery(query, new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);
        }

        #region Rezultati pretrazivanja
        //vraca sve elemente koji sadrze string input u sebu (case-insensitive)
        public List<Album> vratiAlbumeSearch(GraphClient client, string input)
        {
            //var qu = new Neo4jClient.Cypher.CypherQuery("match (n:Album) where n.name=~'.*(?i)" + input + ".*' return n",
            //                                                new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);

            string query = "match (n:Album) where n.name=~'.*(?i)" + input + ".*' return n";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }
        public List<Bend> vratiBendoveSearch(GraphClient client, string input)
        {
            //var qu = new Neo4jClient.Cypher.CypherQuery("match (n:Bend) where n.name=~'.*(?i)" + input + ".*' return n",
            //                                                new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);

            string query = "match (n:Bend) where n.name=~'.*(?i)" + input + ".*' return n";
            var qu = vratiRezultat(query);
            List<Bend> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Bend>(qu).ToList();
            return vrati;
        }
        public List<Izvodjac> vratiIzvodjaceSearch(GraphClient client, string input)
        {
            //var qu = new Neo4jClient.Cypher.CypherQuery("match (n:Izvodjac) where n.name=~'.*(?i)" + input + ".*' return n",
            //                                                new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);

            string query = "match (n:Izvodjac) where n.name=~'.*(?i)" + input + ".*' return n";
            var qu = vratiRezultat(query);
            List<Izvodjac> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Izvodjac>(qu).ToList();
            return vrati;
        }
        public List<Pesma> vratiPesmeSearch(GraphClient client, string input)
        {
            //var qu = new Neo4jClient.Cypher.CypherQuery("match (n:Pesma) where n.name=~'.*(?i)" + input + ".*' return n",
            //                                                new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);

            string query = "match (n:Pesma) where n.name=~'.*(?i)" + input + ".*' return n";
            var qu = vratiRezultat(query);
            List<Pesma> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Pesma>(qu).ToList();
            return vrati;
        }
        public List<Producent> vratiProducentSearch(GraphClient client, string input)
        {
            //var qu = new Neo4jClient.Cypher.CypherQuery("match (n:Producent) where n.name=~'.*(?i)" + input + ".*' return n",
            //                                                new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);

            string query = "match (n:Producent) where n.name=~'.*(?i)" + input + ".*' return n";
            var qu = vratiRezultat(query);
            List<Producent> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Producent>(qu).ToList();
            return vrati;
        }
        #endregion


        #region Prikaz

        public List<Pesma> svePesmeAlbuma(GraphClient client, int id)
        {
            //var qu = new Neo4jClient.Cypher.CypherQuery("match(n:Album) where id(n)="+id+" match(p:Pesma) where (p)-[:DEO]-(a) return p",
            //                                                new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);

            string query = "match(a:Album) where id(a)=" + id + " match(p:Pesma) where (p)-[:DEO]->(a) return p";
            var qu = vratiRezultat(query);
            List<Pesma> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Pesma>(qu).ToList();
            return vrati;
        }

        public List<Izvodjac> sviClanoviGrupe(GraphClient client, int id)
        {
            //var qu = new Neo4jClient.Cypher.CypherQuery("match(n:Album) where id(n)=" + id + " match(p:Pesma) where (p)-[:DEO]-(a) return p",
            //                                                   new Dictionary<string, object>(), Neo4jClient.Cypher.CypherResultMode.Set);

            string query = "match(b:Bend) where id(b)=" + id + " match(i:Izvodjac) where (i)-[:CLAN]->(b) return i";
            var qu = vratiRezultat(query);
            List<Izvodjac> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Izvodjac>(qu).ToList();
            return vrati;
        }

        public List<Album> sviAlbumiGrupe(GraphClient client, int id)
        {
            string query = "match(b:Bend) where id(b)=" + id + " match(a:Album) where (b)-[:IZDAO]->(a) return a";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }

        public List<Producent> sviProducentiAlbuma(GraphClient client, int id)
        {
            string query = "match(a:Album) where id(a)=" + id + " match(p:Producent) where (p)-[:PRODUCIRAO]->(a) return p";
            var qu = vratiRezultat(query);
            List<Producent> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Producent>(qu).ToList();
            return vrati;
        }

        public List<Album> sviAlbumiProducenta(GraphClient client, int id)
        {
            string query = "match(p:Producent) where id(p)=" + id + " match(a:Album) where (p)-[:PRODUCIRAO]->(a) return a";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }

        public List<Bend> bendKojiJeIzdao(GraphClient client, int id)
        {
            string query = "match (a:Album) where id(a)=" + id + " match (b:Bend) where (b)-[:IZDAO]->(a) return b";
            var qu = vratiRezultat(query);
            List<Bend> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Bend>(qu).ToList();
            return vrati;
        }
        
        public List<Album> vratiAlbumPoID(GraphClient client, int id)
        {
            string query = "match (a:Album) where id(a)=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Album> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Album>(qu).ToList();
            return vrati;
        }

        public List<Bend> vratiBendPoID(GraphClient client, int id)
        {
            string query = "match (a:Bend) where id(a)=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Bend> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Bend>(qu).ToList();
            return vrati;
        }

        public List<Izvodjac> vratiIzvodjacaPoID(GraphClient client, int id)
        {
            string query = "match (a:Izvodjac) where id(a)=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Izvodjac> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Izvodjac>(qu).ToList();
            return vrati;
        }

        public List<Producent> vratiProducentaPoID(GraphClient client, int id)
        {
            string query = "match (a:Producent) where id(a)=" + id + " return a";
            var qu = vratiRezultat(query);
            List<Producent> vrati = ((IRawGraphClient)client).ExecuteGetCypherResults<Producent>(qu).ToList();
            return vrati;
        }

        #endregion
    }
}