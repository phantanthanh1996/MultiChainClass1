﻿using Class.MultichainLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class.Lab9.ConsoleApp
{
    class Program
    {
        
        static string ReadAsHexadecimal(MemoryStream bytestream, int count)
        {
            var bytes = new byte[count];
            bytestream.Read(bytes, 0, count);
            return BitConverter.ToString(bytes, 0).Replace("-", "");
        }
        static string ReadAsUInt32(MemoryStream bytestream)
        {
            var bytes = new byte[4];
            bytestream.Read(bytes, 0, 4);
            return BitConverter.ToUInt32(bytes, 0).ToString();
        }
        static string ReadAsUInt64(MemoryStream bytestream)
        {
            var bytes = new byte[8];
            bytestream.Read(bytes, 0, 8);
            return BitConverter.ToUInt64(bytes, 0).ToString();
        }
        static string ReadAsVarInt(MemoryStream bytestream)
        {
            var type = bytestream.ReadByte();
            byte[] result = null;
            switch (type)
            {
                case 0xfd:
                    result = new byte[2];
                    bytestream.Read(result, 0, 2);
                    return BitConverter.ToUInt16(result, 0).ToString();
                case 0xfe:
                    result = new byte[4];
                    bytestream.Read(result, 0, 4);
                    return BitConverter.ToUInt32(result, 0).ToString();
                case 0xff:
                    result = new byte[8];
                    bytestream.Read(result, 0, 8);
                    return BitConverter.ToUInt64(result, 0).ToString();
            }
            return type.ToString();
        }
        static void ParseScript(string script)
        {
            byte[] bytes = Hex.Hex2Bytes(script);
            for (int i = 0; i < bytes.Length; i++)
            {
                var op = bytes[i];
                switch (op)
                {
                    case 0x76:
                        Console.WriteLine("\t\t\t\t" + op.ToString("x") + ": OP_DUP");
                        break;
                    // insert more
                    case 0xa9:
                        Console.WriteLine("\t\t\t\t" + op.ToString("x") + ": OP_HASH160");
                        break;
                    default:
                        if (op >= 1 && op <= 0x4b)
                        {
                            Console.Write("\t\t\t\t" + op.ToString("x") + ": OP_PUSHDATA ");
                            i++;
                            var len = op;
                            ParseMultiChainMetaData(Hex.Bytes2Hex(Hex.SubBytes(bytes, i, len)));
                            i = i + len - 1;
                        }
                        else
                        if (op >= 0x52 && op <= 0x58)
                        {
                            Console.WriteLine("\t\t\t\t" + op.ToString("x") + ": PUSH " + (bytes[i] - 0x51));
                        }
                        break;
                }
            }
        }
        static void ParseMultiChainMetaData(string metadata)
        {
            Console.WriteLine(metadata);
        }

        static void Main(string[] args)
        {
            string a = "01000000011cf11c70af8e0d22e754e29f03a9aff7a5b0d32e775ede4a1fd4b854d3f457ef010000006b483045022100eb8199b6b28c10d51ae3779c244882c08270cec61e0b8364c4f666cf40936427022019ce2f35acb19da41939677e80bd723f829d2cc0264315df0013a1fbf5e9eb410121039f26469516b362a8e513d964eaf924d9a097465c6f2884f6a7b38f0478ab60c9ffffffff0300000000000000002f76a91459bff3a18e12c58a4c137bf6b0807c6b444105a988ac1473706b700010000000000000ffffffff59aee9587500000000000000001976a9140dc4b85e74f5c9a8e2e58e6b9ace962d9d0dc03d88ac0000000000000000252273706b69000000001976a9140dc4b85e74f5c9a8e2e58e6b9ace962d9d0dc03d88ac756a00000000";
            string b = "0100000001477f9cf9366d46c8e5694842ade098182959dd5d3f708cc3e9944323b00590f3010000006b483045022100835f29e757fbae3ec0686815a554f53ec1f0f6c11202c22de47b58fbbda7516c02203ed281a77ef973b714141b6e38875e6dc92eb24ad8b1bf41a9cc8a5863eed8880121039f26469516b362a8e513d964eaf924d9a097465c6f2884f6a7b38f0478ab60c9ffffffff020000000000000000120f73706b6e0200010773747265616d31756a00000000000000001976a9140dc4b85e74f5c9a8e2e58e6b9ace962d9d0dc03d88ac00000000";
            string c = "01000000013a13bd9242b854ec80e8f8c21bd8e8aa8a0c169746080418e12c1b64c20813410100000000ffffffff0100000000000000003776a914e81b3344e96227718756af533c172d8bc83258a888ac1c73706b71ed053a90f4a099acfc2b0c217dd2c05401000000000000007500000000";
            string d = "0100000001bdcab4f5c25284f5e3aa8e3df0e97f5a016034410c60945b4f9921d95a74ca07010000006a47304402200ca6a7d70aa30252685c9af83a82da41eda428f8ff4cd2ba96a489c5c028db8f02202a3290668dea8e6e5421eb130af689ec595bf7186bb5b8c1a1d1a8fc623272920121039f26469516b362a8e513d964eaf924d9a097465c6f2884f6a7b38f0478ab60c9ffffffff0300000000000000002776a9140dc4b85e74f5c9a8e2e58e6b9ace962d9d0dc03d88ac0c73706b6710270000000000007500000000000000001c1973706b6e010001066173736574320041040100000000020101756a00000000000000001976a9140dc4b85e74f5c9a8e2e58e6b9ace962d9d0dc03d88ac00000000";
            string e = "0100000001f08e54df647b8470055ede08005252735f58ee711fb34af3a60492b4bd784c42000000006b483045022100f26794c65ae8d2bd8d3d2b4aa69dc1b98c957870ff28143da182f76671e3c3540220134c96080c713073d48f5e0bb07ba3505163b1f5490cfdf57bf8e13705177626832103ec79c4c7dedd4a3c77a548cd8e59bfc4a378d97f60a455fa8dceba8edcd8a4b7ffffffff0100000000000000003776a914e81b3344e96227718756af533c172d8bc83258a888ac1c73706b716a46a9a3370a9265cb955fcde73daf3601000000000000007500000000";
            string f = "0100000002f08e54df647b8470055ede08005252735f58ee711fb34af3a60492b4bd784c42000000006b483045022100f26794c65ae8d2bd8d3d2b4aa69dc1b98c957870ff28143da182f76671e3c3540220134c96080c713073d48f5e0bb07ba3505163b1f5490cfdf57bf8e13705177626832103ec79c4c7dedd4a3c77a548cd8e59bfc4a378d97f60a455fa8dceba8edcd8a4b7ffffffff6e9b8e708b5e083b81af31938464fb9129f5e7c8adb61bac5bfb88a12833e87e000000006b483045022100dcfa030db9b13371553d0acfe33903e64590f3b64147cb6be5deb1568db82005022004f7347bfe7179e09441e4458df1cf8fb239bba981a6d1a9762a0cc526445fff832103a1d6b7d7be2d817f080d1d914bf84ceacf48303e66ddf12dab9f6146a8fffb0bffffffff0200000000000000003776a914e81b3344e96227718756af533c172d8bc83258a888ac1c73706b716a46a9a3370a9265cb955fcde73daf3601000000000000007500000000000000003776a914c1c1a71a538de0e791dbeb1d1484a3e24116a6cc88ac1c73706b7113576ef5fcafca4cfa046f012659f23b01000000000000007500000000";
            string g = "0100000001b2b3b04e4ffbf3cd90b18d2aa8493e035ed2d4ae0c645b6892d729490129a30d020000006a47304402207ee13b704f574b00ef6a69c13b9f8b41e0a638849ab668b4f2fabebf3b54d997022039be2c797e16d582dbd055d03ca926d6f0f27ce0262f8d9622480b4bdaec9a6d0121039f26469516b362a8e513d964eaf924d9a097465c6f2884f6a7b38f0478ab60c9ffffffff020000000000000000fd36131473706b65016034410c60945b4f9921d95a74ca07750873706b6b6b657931756a4d1213ffd8ffe000104a46494600010100000100010000ffdb00840009060713131215131213151615171615151515151515151515151515161615151515181d2820181a251d151521312125292b2e2e2e171f3338332d37282d2e2b010a0a0a0e0d0e1a10101a2d251f252d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2d2dffc0001108011b00b203012200021101031101ffc4001c0000020203010100000000000000000000040502030001060708ffc40038100002020102050204030704020300000000010203110421051231415106611371819122a1d107143252b1c1f01542e1f14392233353ffc4001a010003010101010000000000000000000001020304000506ffc40023110002020202020300030000000000000000010211032112310441132251146171ffda000c03010002110311003f00ed79cc66ebac9c91f3ecf7c19b2c48dc605f188b6106712b930ab00ec62b6148d731a732518365b1d1c98126fa0da40fce62613fb9b323a462b8b0f246ab8844286cdd5a7c0546690d1c77d9394ff01e3a225fe9e8dcb5862d4b1df042fdd907c38859a2c0caa792528878a627c8d312a8609a921859545ecd00dfa7e5e9b937068ac66998a68da9205c9b521390fc42d4904d3242d5209a643c6624a3a18f32340fce616e688f0287b15f52e92231429635189266364eb86444b673740f28b643f7390ce105d8c9a19c556c4f93f0ae8a2315b97ab222fd45db02d7a8dfa85654b483f137b6369588a67720395e52ecc8259068e20996a739454ec60b19752d52d89f265382469d84ebb97506b581ea2c6be80b1b88e1eb9a61956be3d1bdce596a5aff009ec5f1d5c63bbebe029b42cb1a67453d4aec6eab3b339e8eadb79ec1ba6b9f517e4760789505eb6b4b7408a41f7c32b22fc63a867dd9d0ea8b132fa983c184560f6332fc9868c1ec9967219c84b2664712d91f864ab7be110b6784095ebd47a8cb4076c65a8d4282126af8bb6f628d76b72f2df539ed4ddcd672a7f4fd49c9b97f85b1e34bbec737eb1c9e3fa3270b81ab8b5d11746adfa93aa2b614a592c803c362fe639019a847765b0f1f3299bee67c5df1f438e2c945035ed11b6eedef829b37ee2d8c902de92dfab0782df32ddf8ff818460b062d3c4e4c2669ac5df2fec1f0b3c20375e3a7e417a78e7a86ec4637d3e5c7a8bf50bf10cf48b635aed2a6b296e55c5b8908cd290b60822b06817c0822cc24c359307d9225cc47e21b9b2ac94b39235ab93e5399e23aa71e9b9d26ab78b38dd76a145b4fa1571b160e983ffa872a6f1bbe81dc32858e69f57bf51526a728a4bb8feb8616d164daa2d765ae6bb3279cf92a5537ff00382f8c71d7eac9b19134b0bafdc8ca6977ff003aa2139aecf3ede7e5ee0d39aed2cfb3f3f200c8227734f729f8ef3ee0ff0011bc27b75fe9b6e462e5cbbbeabf500da2e773ce37d975e983237ff9e3e66a11c2fcfdd91827bb71c2ef90341b0886a33d3e9fa975797d58155a7cee9e3edf761518b5dff30019737809d1f502e6f9fe65fa697f99027b035a1f56985c375b80e99ec1f4235e3d98720aad824c9555e466f429bc84d7a54831f124dd9d2f26290abe0bf060e7e0a30b7f0ffb25fc9104d9130da463369938e51c8f1ad0b726923b392c4451a8af3b9b312e5132cdd339ee19a169ff000f419a52cefb05d7561328943be704b22d9683b371fc3ff6537d8fce7dbb16392c7f9fdc02f79d97dc8b2a88df67cbe647e0bc65f8dda7f9835dcd1e92fbe595bd7f969bf77fa75168a5846a351ca96fdb7cadfdb7fb99fbdc7197f9fb747f9316d9a852d9e1f8dffafb7b01cef794bc37bb795d30b67d83c427431d52c25159dbabc3ce33bfe664ac4d67aae98f71469ac5ddeffddf519e9b4edacffd81a3ba2da2ced97d431598ff00188d6a92963296ff0070b5294b75bfc9a0707f807241d2bf3eff00545fa4ea209ce4a58c0f38749ec238d0cfa3a4d1741a5285ba390ce935e230650dac910ac99e9c7a3cf97661861830a73782c822ce43783c6a3d772232e9816db079c0cf0577d39ec69f19bba21996ac5f74308557d8d7c87d655b1cff00105d4a678030c8155dd4b12e6df1f7fd05576a71fd9fe845f15ff64377dd9978d9aae8bb5e9744f3f512d95b6fe7f604e35c6a14b7cd24e7e16ec53a4f58d69e2719e3cac3fc9b291f1b2356908fc9c71d3631d453645e7a2eff00a91a6c6df5fc4faf87e3238e1baba35316aa9372c67970d633e72497a76c73c476fa6581fd75228a77b45dc3a1cd24b0faf8e831f585ff00bbe86586f9e6b962d76eedfd931b7a7f82726f63cb2bf59f0695d4c94136d2785d561add63c898da53562e476a91e0b4ea67296f6fc35bbe66e78585df9137bf4e9dce83d1dc4aefde631f8927172e56a4db5f8ba77f3814f1ae0b651672ca2f7dd6df73b4fd9a7a5ecb2d85b28b508b53949ed971fe18af3db3f23d7cd287c767978a335937e8f40d4705794dbddf8419a6e1ee3dc77ab827f42aad1e2492b3d45374474995b0d6a6091ac2f4fe0be25b33e5761902469187a695230330d9136100b634365d0d214e9789c5f70f86a22fb99e3e2c5765e5e43f4423a540faea70860983ea7a178e38c7a24e6df6249b1071587563bbb66c5baa8649668eacb6296ce3f5d5ca5db725c2383c9c9f63a8af44976dcb614b8b4d181699b5cad523c33d6bc3674eaec8cd75c4a2fca6bb7db021ae4fa76ce71db2bbb5f57f73e94e3fe9fd3eb6a4af865afe19ada717ecff00b1c1c3f6714d76a4ac949379c6cb097b9e9e3f2528d33ce9f8edcad32afd9afa72dccac92e5835b79c3f0fc1e8f642aa22b9a718c7cc9e32febd46dc3f46a15251f1b6de0f23f5b718b2bbac94e29b83c4639d947caf7641e3e6edf66984ab5e8eff00fd7f4abff2737bc53925f54873c3b595dcbf04935e51e11a8f504fe1b957d5f2bc75d9bdf63d1bf661a8574e7647f962a4f18cbfa9178ab7455ca2d7675fc5382d7259e5595ba6d27fd4174b39c76c25db6ec7496453427d669b0f288e58b5d0b8a69e99ae67dcd641e372e99dcd3b3c13e256c6554832842cd336c6f44708d78236ccb99d169993089bcc848c2261c71c129b41156be6bb82b36a27007dc3f8b4a4d2c0f1bca39ef4fd39964e8ad1d2384fc46bee857243ed54728477c30c328da0c654cd4a46a3667b7e85764f6c0346e499e7e4c74cd909da1a3bb0b701ae9e7b13cf7c2ffa28d56a76dbaff406d1eba11925292c9d0ec2fa3bfd33c470cf2dfdacf01f898bea82735b4934bf12ed87e51dbae2f18c779617bf838bf517a9eb9c9c62dcdc7aa8ace1bc60d4dd74678c5cdd1e45c972928aada96ebfb9eb9fb30b5515b5297e293ccbe9b24713afbf5137ff00c7155af3d6413c1b86dce4b9ed9fc96c26469a35e1f17239533dda1ae8be8c13886b7117cbbbecbf53cabd2ba7beed65b17759f0e0e4e293786949c777f43d4a9d3a82c19a6a8451a629a2127bcba8753a708505d822aa49a8d8d2968bb4540791a23827366cc51a32647644c30d16246cd18600e38152275e582c6437e0d5e5e701387dc129c47a075c6a9585b1b9155d01f609a8429d4a1c5e85ba8ac7485b135d0166a20c797d62fba3821931a65a1368596b925eff00d109eca32f71fdd5e412dd318e5068d319a62eba8728b5ccfe5939c9f0eb28b55d457ccbff002473fc51fd51d5d8a51e88bf43a9ff006ca3dfa8d09fa6169ada11cb8c695acca4e0fbc5c5e7e5ee4349c6be2bf87a4ae729bdb9dc7118fbaf275b669f4ede5d717f38a61fc36508ed0828fc92417c51a1f9995aa0bf4a7068696aedccd2e67ddbf763076f36e0f18b9752e83e5212db217ec26a0ca8128790cad0d1542498552c9cc8564a46887642644d6491a68a9335930cc18138f37a1e5a3ade1914a2b6396d14332475946c921263c0675c897303c6782754f25a0f44e5d9bb006e41b360b715420b2e8805b11ad90c825d40921e22bb20092632d4c30078cf533ccd1140f95dc8ce508f62cf8257651933b2a89d57c587d1a88aec2fab4d818d1a6c8b61683e3aa6f65d0be8adb2ba29c0c29880e2ea6217029ad1720a62308ad93994d6cb2c9a5d4b6323334660c4cd972668c330600e380e08bf11d1ce673dc256f91b4ec15f6523d04bd485e82cc88dcf718f09b376560a8490d2651345d36513654920599a714cc932a53c0ac7451751b814f4e3596e6be119a6688b13bd3f42d8e8866a82e8568ceca8aff730ba280a759a8ac08312aea08ae2460c9c59c2b65912688459626114b60c55ea1d4ca315ca348339ef55cf182d025203d2f1d9c7a8de8e3f17d4e2e56e3634ee2bc84a3d0171887930f3ff00de7dcc073050cf84a78184d6c2be116ec319c832ec78bd035926837825ff008da17ea2c09f4cbcce4ca4589247472b0a9da8d5cb7049bc32ac9a2cb240d6b3529b26a1e49b65111ae7b17d7340d645a2af8b8252d9488d174322c5cb57b1742f4f06768aa61992b522b94fc15a9ee4c60bad96a058308ae4700ba25a983f3138b08184c19ce7abe4b09aea3fa99c67ac355ffc89782b127212f3f925ce04e6fb1a5a8cf51840f300fe2af2cc38e1c70897e2c1d0cfa1cb682d4a69b674f1d541aea8d5c6c92950bb55041de95861c81f57cbd833d351c7308a3521dcad0d35080ad88cec8816a0ab1100a7b9729f90672c32ab24c9b638667251744a16a362b7ad2522889dc8846ec32b57f311e6c1163a61d0bde3e65f48b6ab3708af5485a1ac65065a98ae1ac590daedca051d6171916c640706cbe1201c1b07b1e71eaab632b9e0f439bfc0fe4794716ba5f164f1dca226ca958d162b13ea81637a7b35865fcdb0d6298daf061af8dec61d67165b7e055afe316d7bc72c633c7707b6b4f6362667685ba6f5c4d37f123247a3fa0f8b7c78b92d97b9e7d3d1433d17d8ec7d0506a4e317f85760b391df3b01aff627283c909a1243c40adaf728baad83268ad90948b2429950c07535b43d9d601aba3249b1e842b5d2adefd08ae2fcdd0af8dc3962d887866a32f03c636ac572a74759a5d7790882cb15e9a233d388c60aae230a278048c7a054624dba1906c265f4304a906d1116c341939620dfb1e55c4accd926bcbfea7a5f17b5c68935d707966a6ce66dbea6882b4464f66492c6e88423e19b8d9db0667bff00439a01997e0d91f8bee60283449ee69c4dcebc1152f26c2045bc6f83b4fd9fa4f99f7f071d94763e8286399ae8ce6723b2b2280e6c26d983496494a4563107995f2854d14b466948b24532883dd00a6c1ee96109d859c57ac5b50c2392e153c4f73a1f576a149b49f4393af5186b3f737e38fd0c9397d8ef742f2348b39be0fac4fb8e55e659aa3447636aa4195bd84fa2b8610b4cd29168c58ca961da788b2990cf4d2114c771338d699ce9925d70796ea6971934f668f608cb63cffd6da55cfcc963dcd38e7e8cd389cb3b71d7ee6e1626f664229f4d9a31457d4a3109f2bf6308f34bc23001a0b954f62a9edd432cdc1edcf8d8d64281d7b1e9fe91d0f2d29f9dcf34ada3d6fd2cf3447e4095d062133a816fd33f238940a2ca4cb34cd31684f34d03c9b1adba703b28c1926e4688d02f2ec03c5a7cb5c9fb0d144e6bd67aaf8753f0f62b836f64b2e91e67c4f59cd37bf917269f7dc2eeae33dd0bada2717d0f5392a3cf699d07a7f0a586f076ba38a6793d3ac9c5f57f53bef4ef114e1972466cf1bd97c52ad1d655420ca6ac02693529ad8650679d386cdd09e89d700ba65806847c97c4558e83298753315fab345f129935d520caa7e4138bf1050aa79f0cd18ccf33cb67ccb6c648d56a795d1fb976339717d5b7b92757f32dcb132ae797b1859c8fc2340a3a871cab00f7d5b64254dbca7f72ae56bdd1a8901c2bfc51c79dcf5ee0904aa8a4b1b1e45a9494935b1ea1e9fe2709d71c3ec87ab42dd31eb5ee5726fce48cb511f620ec8bdd333ce25a32353982db2f62db395f705b6d5e4cb28964c1b537a8ac9e6deb8e27f124a30794ba9d0fab38d46b8b8a7993db08f32bed9e5bfe2653146b626495e8a670efd3e45b54fdf26a3627d5619b9d5e3f22b64a8af535425f3038c6cab1caf6eb80f8d4fb2cfccbbe03c8d6c1483784fab30d46c4d76cf63bce19c6e124b124fea79a4f87a7dbe8caead0ca3fc0daf9311e34c75368f647c4238ce514d1ea0aff00997dcf289597e1c7e2cb18c6dd4061c1f1d273ff00d98ab120fc8cf5ce23eabaab8bfc4b3e0e4f57c76db935d1673f339da386e3f14b2fdf391ad15f2a5feedb283c280e566ddade3997d8b14b2b679fec6a504faec6a757e7dd019d64ff00f63652a33f28c3a8343c693dd3cfb325ba5d3e8537ed8c6c5f54dbea6924516b8bda4b00d5ea6ed3c94eb798f788c6451a8ee329501ab2767ed26108353aa7cde12c94f09fda55734d34e32ec9f8f9816b74f1786e2b22f9682bfe442ca986368e9759fb44aa1d3327e12c81ddebe9c93e5aa5bf77d84b5e8ebfe541fc3b4f1e66b956324382296c4ceb95b272949b937979659fb934f7dbdc77aea631598a49e7b03c1e7aee1510362a7a2eb935569b0b19ce06ea0b2d63605be0974f2512a1594a8657836a09f5dcb1909bc74080c69777f7355fb6304ace9f6324b7471c4b99746bea59f07afbf42c8c563a17c16c0a0d83569e547779ec979e892184b84dd25ffd724fe4d04701ad3d4432bb37f551783aa8d11e98db6eefb740a5606e8e365c36f4b7ae4fc2c6efd8d4785db2c62bb17b38b3b37447c7e6cb3f768b4b6e9eecee00e4711fe9577ff8cbeccc3b34b1b2361e08ee4cffd900000000000000001976a9140dc4b85e74f5c9a8e2e58e6b9ace962d9d0dc03d88ac00000000";

            byte[] buffer = Hex.Hex2Bytes(f);
            using (MemoryStream bytestream = new MemoryStream(buffer))
            {
                //ToDo: 1 - Read Version. Run this and see what happens.
                var ver = ReadAsUInt32(bytestream);
                Console.WriteLine("version: \t=>{0}", ver);

                //ToDo: 2 - Read InputCount. Use the appropriate methods above to parse the rest of the raw string (https://en.bitcoin.it/wiki/Transaction#Pay-to-Script-Hash)
                //var inputCount = int.Parse();
                //Console.WriteLine("InputCount: \t=>{0}", inputCount);

                //ToDo: 3 - Read Inputs
                //for (int i = 0; i < inputCount; i++)
                //{

                //    //ToDo: 4 - Read txid
                //    var txid = 
                //    Console.WriteLine("txid: \t\t\t=>{0}", txid);

                //    //ToDo: 5 - Read vout
                //    var vout = 
                //    Console.WriteLine("vout: \t\t\t=>{0}", vout);

                //    //ToDo: 6 - Read scriptsig length
                //    var scriptsigLen = int.Parse();
                //    Console.WriteLine("scriptsiglen: \t\t=>{0}", scriptsigLen);

                //    //ToDo: 7 - Read scriptsig
                //    var scriptsig = 
                //    Console.WriteLine("scriptsig: \t\t=>{0}", scriptsig);

                //    //ToDo: 8 - Read sequence
                //    var sequence = 
                //    Console.WriteLine("sequence: \t\t=>{0}", sequence);
                //}

                //ToDo: 9 - Read OutputCount
                //int outputCount = int.Parse();
                //Console.WriteLine("OutputCount: \t=>{0}", outputCount);

                //for (int i = 0; i < outputCount; i++)
                //{
                //    //ToDo: 10 - Read Outputs
                //    var val = 
                //    Console.WriteLine("Value: \t\t\t=>{0}", val);

                //    //ToDo: 11 - Read scriptpubkey length
                //    int scriptPubKeyLen = int.Parse();
                //    Console.WriteLine("scriptpubkeylen: \t=>{0}", scriptPubKeyLen);

                //    //ToDo: 12 - Complete the opcodes needed to parse the scriptpubkey in the method ParseScript refer to https://en.bitcoin.it/wiki/Script
                //    string scriptpubkey = 
                //    Console.WriteLine("scriptpubkey:");
                //    ParseScript(scriptpubkey);

                //}

                // ToDo: 13 - Read Lock Time
                //Console.WriteLine("LockTime: \t\t=>{0}", ReadAsUInt32(bytestream));
            }
            Console.ReadLine();

            //ToDo: 14 - Change the method ParseMultiChainMetaData to parse the metadata using http://www.multichain.com/developers/native-assets/

            //ToDo: 15 - Replace buffer with the rest of the rawstring and try to interpret what each scripts are doing.
        }
    }
}
