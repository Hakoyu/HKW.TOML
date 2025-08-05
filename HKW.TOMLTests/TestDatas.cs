using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKW.TOMLTests;

public static class TestDatas
{
    public static string TomlInteger =>
        """
            # 整数
            int1 = +99
            int2 = 42
            int3 = 0
            int4 = -17

            # 十六进制带有前缀 `0x`
            hex1 = 0xDEADBEEF
            hex2 = 0xdeadbeef
            hex3 = 0xdead_beef

            # 八进制带有前缀 `0o`
            oct1 = 0o01234567
            oct2 = 0o755

            # 二进制带有前缀 `0b`
            bin1 = 0b11010110
            """;

    public static string TomlFloat =>
        """
            # 小数
            float1 = +1.0
            float2 = 3.1415
            float3 = -0.01

            # 指数
            float4 = 5e+22
            float5 = 1e06
            float6 = -2E-2

            # both
            float7 = 6.626e-34

            # 分隔符
            float8 = 224_617.445_991_228

            # 无穷
            infinite1 = inf # 正无穷
            infinite2 = +inf # 正无穷
            infinite3 = -inf # 负无穷

            # 非数
            not1 = nan
            not2 = +nan
            not3 = -nan 
            """;

    public static string TomlComment =>
        """
            # 这是一条 TOML 注释
            key1 = 1

            # 这是一个多行的
            # TOML 注释
            key2 = 2

            key3 = 3 # 这是一个行尾注释

            # 这个值同时有顶部注释        
            key4 = 4 # 和行尾注释

            # 这是一个数组内注释
            array = [
            # 项目1注释
            1,
            2, # 项目2注释
            # 这是项目3
            3 # 他有两个注释
            ]
            """;
    public static string TomlString =>
        """"
            str1 = "I'm a string."
            str2 = "You can \"quote\" me."
            str3 = "Name\tJos\u00E9\nLoc\tSF."

            str4 = """
            Roses are red
            Violets are blue"""

            str5 = """\
              The quick brown \
              fox jumps over \
              the lazy dog.\
              """
            path = 'C:\Users\nodejs\templates'
            path2 = '\\User\admin$\system32'
            quoted = 'Tom "Dubs" Preston-Werner'
            regex = '<\i\c*\s*>'

            re = '''I [dw]on't need \d{2} apples'''
            lines = '''
            原始字符串中的
            第一个换行被剔除了。
            所有其它空白
            都保留了。
            '''
            """";

    public static string TomlDateTime =>
        """
            # 坐标日期时刻
            odt1 = 1979-05-27T07:32:00Z
            odt2 = 1979-05-27T00:32:00-07:00
            odt3 = 1979-05-27T00:32:00.999999-07:00

            # 各地日期时刻
            ldt1 = 1979-05-27T07:32:00
            ldt2 = 1979-05-27T00:32:00.999999

            # 各地日期
            ld1 = 1979-05-27

            # 各地时刻
            lt1 = 07:32:00
            lt2 = 00:32:00.999999
            """;
}
