using MongoDB.Driver;

namespace MonitoriaServicosApi.Repository
{
    public class Context
    {
        //Conexão com o MONGODB PROADV
        static readonly string stringConexaoMongo = "mongodb://admin:admin@prorj01mon01:19000,prorj01mon02:19000,prorj01mon03:19000?authSource=admin&replicaSet=rsProAdv&readPreference=SecondaryPreferred";
        private static readonly MongoClient Conexao = new MongoClient(stringConexaoMongo);
        public static IMongoDatabase ConexaoProdMongo = Conexao.GetDatabase("ProAdv");
        public static IMongoDatabase ConexaoProdBipbopMongo = Conexao.GetDatabase("IntegracaoBipBop");


        //Conexão com o MONGODB HOMOLOG
        static readonly string stringConexaoMongoHomolog = "mongodb://10.21.0.67:27017";
        private static readonly MongoClient ConexaoHomolog = new MongoClient(stringConexaoMongoHomolog);
        public static IMongoDatabase ConexaoHomologMongo = ConexaoHomolog.GetDatabase("dashboard_v2");
    }
}