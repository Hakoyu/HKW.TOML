namespace HKW.HKWTOML.Benchmark;

public static class Example
{
    //public static string ExampleFile { get; set; } = "..\\..\\..\\Example.toml";
    public static string TomlExampleSourceData =>
        """"
            # TOML 测试数据 - 覆盖所有数据类型和格式
            # 基于 TOML v1.0.0 规范

            # ============ 基本字符串 ============
            title = "TOML 测试数据集"
            description = "这是一个包含所有 TOML 数据类型的测试文件"
            basic_string = "Hello, World!"
            unicode_string = "你好世界 🌍"
            empty_string = ""

            # ============ 多行字符串 ============
            multiline_basic1 = """
            这是一个多行
            基本字符串
            可以包含换行符
            """

            multiline_basic2 = """
            这是一个多行\
            转换为单行的字符串\
            使用反斜杠标记行尾来连成一行\
            """

            multiline_literal = '''
            这是一个多行
            字面字符串
            不会转义反斜杠 \n \t
            '''

            # ============ 字面字符串 ============
            literal_string = 'C:\Users\nodejs\templates'
            windows_path = 'C:\Windows\System32'
            regex_pattern = '<\i\c*\s*>'

            # ============ 转义字符串 ============
            escaped_string = "转义字符: \n\t\r\"\\"
            unicode_escape = "\u4E2D\u6587"

            # ============ 整数 ============
            positive_int = 42
            negative_int = -17
            zero_int = 0
            underscore_int = 1_000_000
            hex_int = 0xDEADBEEF
            octal_int = 0o755
            binary_int = 0b11010110

            # ============ 浮点数 ============
            positive_float = 3.14159
            negative_float = -0.01
            exponent_float = 5e+22
            negative_exponent = 1e-17
            underscore_float = 9_224_617.445_991_228_313
            infinity = inf
            negative_infinity = -inf
            not_a_number = nan

            # ============ 布尔值 ============
            is_enabled = true
            is_disabled = false

            # ============ 日期时间 ============
            # RFC 3339 格式
            offset_datetime = 1979-05-27T07:32:00Z
            local_datetime = 1979-05-27T07:32:00
            local_date = 1979-05-27
            local_time = 07:32:00

            # 带毫秒的日期时间
            precise_datetime = 1979-05-27T00:32:00.999999Z
            timezone_datetime = 1979-05-27T07:32:00-08:00

            # ============ 数组 ============
            simple_array = [1, 2, 3]
            string_array = ["red", "yellow", "green"]
            nested_array = [[1, 2], ["a", "b", "c"]]
            mixed_array = [1, "hello", true, 3.14]
            empty_array = []

            # 多行数组
            multiline_array = [1, 2, 3, 4, 5]

            # 数组中的数组
            array_of_arrays = [[1, 2, 3], ["a", "b", "c"], [true, false]]

            # ============ 内联表 ============
            inline_table = { name = "John Doe", age = 30, city = "New York" }
            empty_inline_table = {}
            nested_inline = { point = { x = 1, y = 2 }, color = "red" }

            # ============ 表 ============
            [database]
            server = "192.168.1.1"
            ports = [8001, 8001, 8002]
            connection_max = 5000
            enabled = true

            [database.credentials]
            username = "admin"
            password = "secret123"

            # ============ 表数组 ============
            [[products]]
            name = "Hammer"
            sku = 738594937
            price = 12.99
            tags = ["hardware", "tools"]

            [[products]]
            name = "Nail"
            sku = 284758393
            price = 0.99
            tags = ["hardware", "fasteners"]

            # ============ 嵌套表 ============
            [clients]
            [clients.data]
            name = "Google"
            hosts = ["alpha", "omega"]

            [clients.hosts]
            alpha = "10.0.0.1"
            omega = "10.0.0.2"

            # ============ 复杂嵌套结构 ============
            [servers]

            [servers.alpha]
            ip = "10.0.0.1"
            dc = "eqdc10"
            country = "中国"

            [servers.beta]
            ip = "10.0.0.2"
            dc = "eqdc10"
            country = "美国"

            # ============ 表数组的复杂示例 ============
            [[fruit]]
            name = "apple"
            color = "red"
            shape = "round"

            [[fruit.variety]]
            name = "red delicious"
            sweetness = 8

            [[fruit.variety]]
            name = "granny smith"
            sweetness = 4

            [[fruit]]
            name = "banana"
            color = "yellow"
            shape = "curved"

            [[fruit.variety]]
            name = "cavendish"
            sweetness = 9

            # ============ 特殊值测试 ============
            [special_values]
            empty_string = ""
            zero_integer = 0
            zero_float = 0.0
            positive_infinity = inf
            negative_infinity = -inf
            not_a_number = nan
            true_boolean = true
            false_boolean = false
            empty_array = []
            empty_inline_table = {}

            # ============ 注释测试 ============
            # 这是一个注释
            commented_key1 = "value"

            commented_key2 = "value" # 行尾注释

            # 多行注释
            # 可以有多行
            # 每行都以 # 开头
            commented_key3 = "with value"

            # 注释
            # 可以有多行
            commented_key4 = "with value" # 但行尾注释只能有一行

            # 这是一个带有注释的数组
            commented_array1 = [
                0,
                # 值可以有注释
                1,
                2, # 值可以有行尾注释
                # 值可以同时有
                3, # 两个注释
                4,
                5,
            ] # 可以在数组末尾注释

            commented_array2 = [1, 2, 3] # 单行数组可以有行尾注释

            commented_table = { value = 1 } # 单行表格可以有行尾注释

            # ============ 复杂数据结构 ============
            [config]
            version = "1.0.0"
            debug = true
            max_connections = 100
            timeout = 30.5

            [config.logging]
            level = "info"
            file = "/var/log/app.log"
            rotate = true
            max_size = "10MB"

            [config.features]
            authentication = true
            caching = false
            compression = true

            [[config.endpoints]]
            name = "api"
            url = "https://api.example.com"
            methods = ["GET", "POST", "PUT", "DELETE"]
            timeout = 5000

            [[config.endpoints]]
            name = "webhook"
            url = "https://webhook.example.com"
            methods = ["POST"]
            timeout = 3000

            # ============ 国际化测试 ============
            [i18n]
            english = "Hello, World!"
            chinese = "你好，世界！"
            japanese = "こんにちは、世界！"
            korean = "안녕하세요, 세계!"
            arabic = "مرحبا بالعالم!"
            russian = "Привет, мир!"
            emoji = "🌍🌎🌏"

            # ============ 数据类型边界测试 ============
            [boundaries]
            max_int = 9223372036854775807
            min_int = -9223372036854775808
            small_float = 1.23e-10
            large_float = 1.23e+10
            very_long_string = "这是一个非常长的字符串，用来测试 TOML 解析器对长字符串的处理能力。它包含了中文字符、英文字符、数字和特殊符号！@#$%^&*()_+-=[]{}|;':\",./<>?"

            # ============ 引用和转义测试 ============
            [quotes_and_escapes]
            single_quote = "包含'单引号'的字符串"
            double_quote = '包含"双引号"的字符串'
            backslash = "包含\\反斜杠的字符串"
            newline = "包含\n换行符的字符串"
            tab = "包含\t制表符的字符串"
            carriage_return = "包含\r回车符的字符串"
            unicode = "Unicode字符: \u4E2D\u6587"

            """";

    public static string TomlExampleData =>
        """"
            # TOML 测试数据 - 覆盖所有数据类型和格式
            # 基于 TOML v1.0.0 规范

            # ============ 基本字符串 ============
            title = "TOML 测试数据集"
            description = "这是一个包含所有 TOML 数据类型的测试文件"
            basic_string = "Hello, World!"
            unicode_string = "你好世界 🌍"
            empty_string = ""
            # ============ 多行字符串 ============
            multiline_basic1 = """
            这是一个多行
            基本字符串
            可以包含换行符
            """
            multiline_basic2 = """这是一个多行转换为单行的字符串使用反斜杠标记行尾来连成一行"""
            multiline_literal = '''
            这是一个多行
            字面字符串
            不会转义反斜杠 \n \t
            '''
            # ============ 字面字符串 ============
            literal_string = 'C:\Users\nodejs\templates'
            windows_path = 'C:\Windows\System32'
            regex_pattern = '<\i\c*\s*>'
            # ============ 转义字符串 ============
            escaped_string = "转义字符: \n\t\r\"\\"
            unicode_escape = "中文"
            # ============ 整数 ============
            positive_int = 42
            negative_int = -17
            zero_int = 0
            underscore_int = 1000000
            hex_int = 0xdeadbeef
            octal_int = 0o755
            binary_int = 0b11010110
            # ============ 浮点数 ============
            positive_float = 3.14159
            negative_float = -0.01
            exponent_float = 5e+22
            negative_exponent = 1e-17
            underscore_float = 9224617.445991227
            infinity = inf
            negative_infinity = -inf
            not_a_number = nan
            # ============ 布尔值 ============
            is_enabled = true
            is_disabled = false
            # ============ 日期时间 ============
            # RFC 3339 格式
            offset_datetime = 1979-05-27T07:32:00Z
            local_datetime = 1979-05-27T07:32:00
            local_date = 1979-05-27
            local_time = 07:32:00
            # 带毫秒的日期时间
            precise_datetime = 1979-05-27T00:32:00.999999Z
            timezone_datetime = 1979-05-27T07:32:00-08:00
            # ============ 数组 ============
            simple_array = [ 1, 2, 3 ]
            string_array = [ "red", "yellow", "green" ]
            nested_array = [ [ 1, 2 ], [ "a", "b", "c" ] ]
            mixed_array = [ 1, "hello", true, 3.14 ]
            empty_array = []
            # 多行数组
            multiline_array = [ 1, 2, 3, 4, 5 ]
            # 数组中的数组
            array_of_arrays = [ [ 1, 2, 3 ], [ "a", "b", "c" ], [ true, false ] ]

            [[products]]
            # ============ 表数组 ============
            name = "Hammer"
            sku = 738594937
            price = 12.99
            tags = [ "hardware", "tools" ]

            [[products]]
            name = "Nail"
            sku = 284758393
            price = 0.99
            tags = [ "hardware", "fasteners" ]

            [[fruit]]
            # ============ 表数组的复杂示例 ============
            name = "apple"
            color = "red"
            shape = "round"

            [[fruit.variety]]
            name = "red delicious"
            sweetness = 8

            [[fruit.variety]]
            name = "granny smith"
            sweetness = 4

            [[fruit]]
            name = "banana"
            color = "yellow"
            shape = "curved"

            [[fruit.variety]]
            name = "cavendish"
            sweetness = 9
            # ============ 内联表 ============
            inline_table = { name = "John Doe", age = 30, city = "New York" }
            empty_inline_table = {}
            nested_inline = { point = { x = 1, y = 2 }, color = "red" }

            # ============ 表 ============
            [database]
            server = "192.168.1.1"
            ports = [ 8001, 8001, 8002 ]
            connection_max = 5000
            enabled = true

            [database.credentials]
            username = "admin"
            password = "secret123"

            # ============ 嵌套表 ============
            [clients]
            [clients.data]
            name = "Google"
            hosts = [ "alpha", "omega" ]

            [clients.hosts]
            alpha = "10.0.0.1"
            omega = "10.0.0.2"

            # ============ 复杂嵌套结构 ============
            [servers]
            [servers.alpha]
            ip = "10.0.0.1"
            dc = "eqdc10"
            country = "中国"

            [servers.beta]
            ip = "10.0.0.2"
            dc = "eqdc10"
            country = "美国"

            # ============ 特殊值测试 ============
            [special_values]
            empty_string = ""
            zero_integer = 0
            zero_float = 0
            positive_infinity = inf
            negative_infinity = -inf
            not_a_number = nan
            true_boolean = true
            false_boolean = false
            empty_array = []
            # ============ 注释测试 ============
            # 这是一个注释
            commented_key1 = "value"
            commented_key2 = "value"
            # 多行注释
            # 可以有多行
            # 每行都以 # 开头
            commented_key3 = "with value"
            # 注释
            # 可以有多行
            commented_key4 = "with value"
            # 这是一个带有注释的数组
            commented_array1 = [
              0,
              # 值可以有注释
              1,
              2, # 值可以有行尾注释
              # 值可以同时有
              3, # 两个注释
              4,
              5,

            ] # 可以在数组末尾注释
            commented_array2 = [ 1, 2, 3 ] # 单行数组可以有行尾注释
            empty_inline_table = {}
            commented_table = { value = 1 } # 单行表格可以有行尾注释

            # ============ 复杂数据结构 ============
            [config]
            version = "1.0.0"
            debug = true
            max_connections = 100
            timeout = 30.5

            [[config.endpoints]]
            name = "api"
            url = "https://api.example.com"
            methods = [ "GET", "POST", "PUT", "DELETE" ]
            timeout = 5000

            [[config.endpoints]]
            name = "webhook"
            url = "https://webhook.example.com"
            methods = [ "POST" ]
            timeout = 3000

            [config.logging]
            level = "info"
            file = "/var/log/app.log"
            rotate = true
            max_size = "10MB"

            [config.features]
            authentication = true
            caching = false
            compression = true

            # ============ 国际化测试 ============
            [i18n]
            english = "Hello, World!"
            chinese = "你好，世界！"
            japanese = "こんにちは、世界！"
            korean = "안녕하세요, 세계!"
            arabic = "مرحبا بالعالم!"
            russian = "Привет, мир!"
            emoji = "🌍🌎🌏"

            # ============ 数据类型边界测试 ============
            [boundaries]
            max_int = 9223372036854775807
            min_int = -9223372036854775808
            small_float = 1.23e-10
            large_float = 12300000000
            very_long_string = "这是一个非常长的字符串，用来测试 TOML 解析器对长字符串的处理能力。它包含了中文字符、英文字符、数字和特殊符号！@#$%^&*()_+-=[]{}|;':\",./<>?"

            # ============ 引用和转义测试 ============
            [quotes_and_escapes]
            single_quote = "包含'单引号'的字符串"
            double_quote = '包含"双引号"的字符串'
            backslash = "包含\\反斜杠的字符串"
            newline = "包含\n换行符的字符串"
            tab = "包含\t制表符的字符串"
            carriage_return = "包含\r回车符的字符串"
            unicode = "Unicode字符: 中文"

            """";

    public static string TomlExampleDataCompatibleJson =>
        """"
            # TOML 测试数据 - 覆盖所有数据类型和格式
            # 基于 TOML v1.0.0 规范

            # ============ 基本字符串 ============
            title = "TOML 测试数据集"
            description = "这是一个包含所有 TOML 数据类型的测试文件"
            basic_string = "Hello, World!"
            unicode_string = "你好世界 🌍"
            empty_string = ""

            # ============ 多行字符串 ============
            multiline_basic1 = """
            这是一个多行
            基本字符串
            可以包含换行符
            """

            multiline_basic2 = """
            这是一个多行\
            转换为单行的字符串\
            使用反斜杠标记行尾来连成一行\
            """

            multiline_literal = '''
            这是一个多行
            字面字符串
            不会转义反斜杠 \n \t
            '''

            # ============ 字面字符串 ============
            literal_string = 'C:\Users\nodejs\templates'
            windows_path = 'C:\Windows\System32'
            regex_pattern = '<\i\c*\s*>'

            # ============ 转义字符串 ============
            escaped_string = "转义字符: \n\t\r\"\\"
            unicode_escape = "\u4E2D\u6587"

            # ============ 整数 ============
            positive_int = 42
            negative_int = -17
            zero_int = 0
            underscore_int = 1_000_000
            hex_int = 0xDEADBEEF
            octal_int = 0o755
            binary_int = 0b11010110

            # ============ 浮点数 ============
            positive_float = 3.14159
            negative_float = -0.01
            exponent_float = 5e+22
            negative_exponent = 1e-17
            underscore_float = 9_224_617.445_991_228_313
            infinity = 0
            negative_infinity = 0
            not_a_number = 0

            # ============ 布尔值 ============
            is_enabled = true
            is_disabled = false

            # ============ 数组 ============
            simple_array = [1, 2, 3]
            string_array = ["red", "yellow", "green"]
            nested_array = [[1, 2], ["a", "b", "c"]]
            mixed_array = [1, "hello", true, 3.14]
            empty_array = []

            # 多行数组
            multiline_array = [1, 2, 3, 4, 5]

            # 数组中的数组
            array_of_arrays = [[1, 2, 3], ["a", "b", "c"], [true, false]]

            # ============ 内联表 ============
            inline_table = { name = "John Doe", age = 30, city = "New York" }
            empty_inline_table = {}
            nested_inline = { point = { x = 1, y = 2 }, color = "red" }

            # ============ 表 ============
            [database]
            server = "192.168.1.1"
            ports = [8001, 8001, 8002]
            connection_max = 5000
            enabled = true

            [database.credentials]
            username = "admin"
            password = "secret123"

            # ============ 表数组 ============
            [[products]]
            name = "Hammer"
            sku = 738594937
            price = 12.99
            tags = ["hardware", "tools"]

            [[products]]
            name = "Nail"
            sku = 284758393
            price = 0.99
            tags = ["hardware", "fasteners"]

            # ============ 嵌套表 ============
            [clients]
            [clients.data]
            name = "Google"
            hosts = ["alpha", "omega"]

            [clients.hosts]
            alpha = "10.0.0.1"
            omega = "10.0.0.2"

            # ============ 复杂嵌套结构 ============
            [servers]

            [servers.alpha]
            ip = "10.0.0.1"
            dc = "eqdc10"
            country = "中国"

            [servers.beta]
            ip = "10.0.0.2"
            dc = "eqdc10"
            country = "美国"

            # ============ 表数组的复杂示例 ============
            [[fruit]]
            name = "apple"
            color = "red"
            shape = "round"

            [[fruit.variety]]
            name = "red delicious"
            sweetness = 8

            [[fruit.variety]]
            name = "granny smith"
            sweetness = 4

            [[fruit]]
            name = "banana"
            color = "yellow"
            shape = "curved"

            [[fruit.variety]]
            name = "cavendish"
            sweetness = 9

            # ============ 特殊值测试 ============
            [special_values]
            empty_string = ""
            zero_integer = 0
            zero_float = 0.0
            positive_infinity = 0
            negative_infinity = 0
            not_a_number = 0
            true_boolean = true
            false_boolean = false
            empty_array = []
            empty_inline_table = {}

            # ============ 注释测试 ============
            # 这是一个注释
            commented_key1 = "value"

            commented_key2 = "value" # 行尾注释

            # 多行注释
            # 可以有多行
            # 每行都以 # 开头
            commented_key3 = "with value"

            # 注释
            # 可以有多行
            commented_key4 = "with value" # 但行尾注释只能有一行

            # 这是一个带有注释的数组
            commented_array1 = [
                0,
                # 值可以有注释
                1,
                2, # 值可以有行尾注释
                # 值可以同时有
                3, # 两个注释
                4,
                5,
            ] # 可以在数组末尾注释

            commented_array2 = [1, 2, 3] # 单行数组可以有行尾注释

            commented_table = { value = 1 } # 单行表格可以有行尾注释

            # ============ 复杂数据结构 ============
            [config]
            version = "1.0.0"
            debug = true
            max_connections = 100
            timeout = 30.5

            [config.logging]
            level = "info"
            file = "/var/log/app.log"
            rotate = true
            max_size = "10MB"

            [config.features]
            authentication = true
            caching = false
            compression = true

            [[config.endpoints]]
            name = "api"
            url = "https://api.example.com"
            methods = ["GET", "POST", "PUT", "DELETE"]
            timeout = 5000

            [[config.endpoints]]
            name = "webhook"
            url = "https://webhook.example.com"
            methods = ["POST"]
            timeout = 3000

            # ============ 国际化测试 ============
            [i18n]
            english = "Hello, World!"
            chinese = "你好，世界！"
            japanese = "こんにちは、世界！"
            korean = "안녕하세요, 세계!"
            arabic = "مرحبا بالعالم!"
            russian = "Привет, мир!"
            emoji = "🌍🌎🌏"

            """";

    public static string JsonExampleData =>
        """
            {
              "title": "TOML 测试数据集",
              "description": "这是一个包含所有 TOML 数据类型的测试文件",
              "basic_string": "Hello, World!",
              "unicode_string": "你好世界 🌍",
              "empty_string": "",
              "multiline_basic1": "这是一个多行\r\n基本字符串\r\n可以包含换行符\r\n",
              "multiline_basic2": "这是一个多行转换为单行的字符串使用反斜杠标记行尾来连成一行",
              "multiline_literal": "这是一个多行\r\n字面字符串\r\n不会转义反斜杠 \\n \\t\r\n",
              "literal_string": "C:\\Users\\nodejs\\templates",
              "windows_path": "C:\\Windows\\System32",
              "regex_pattern": "<\\i\\c*\\s*>",
              "escaped_string": "转义字符: \n\t\r\"\\",
              "unicode_escape": "中文",
              "positive_int": 42,
              "negative_int": -17,
              "zero_int": 0,
              "underscore_int": 1000000,
              "hex_int": 3735928559,
              "octal_int": 493,
              "binary_int": 214,
              "positive_float": 3.14159,
              "negative_float": -0.01,
              "exponent_float": 5e+22,
              "negative_exponent": 1e-17,
              "underscore_float": 9224617.445991227,
              "infinity": 0,
              "negative_infinity": 0,
              "not_a_number": 0,
              "is_enabled": true,
              "is_disabled": false,
              "simple_array": [
                1,
                2,
                3
              ],
              "string_array": [
                "red",
                "yellow",
                "green"
              ],
              "nested_array": [
                [
                  1,
                  2
                ],
                [
                  "a",
                  "b",
                  "c"
                ]
              ],
              "mixed_array": [
                1,
                "hello",
                true,
                3.14
              ],
              "empty_array": [],
              "multiline_array": [
                1,
                2,
                3,
                4,
                5
              ],
              "array_of_arrays": [
                [
                  1,
                  2,
                  3
                ],
                [
                  "a",
                  "b",
                  "c"
                ],
                [
                  true,
                  false
                ]
              ],
              "inline_table": {
                "name": "John Doe",
                "age": 30,
                "city": "New York"
              },
              "empty_inline_table": {},
              "nested_inline": {
                "point": {
                  "x": 1,
                  "y": 2
                },
                "color": "red"
              },
              "database": {
                "server": "192.168.1.1",
                "ports": [
                  8001,
                  8001,
                  8002
                ],
                "connection_max": 5000,
                "enabled": true,
                "credentials": {
                  "username": "admin",
                  "password": "secret123"
                }
              },
              "products": [
                {
                  "name": "Hammer",
                  "sku": 738594937,
                  "price": 12.99,
                  "tags": [
                    "hardware",
                    "tools"
                  ]
                },
                {
                  "name": "Nail",
                  "sku": 284758393,
                  "price": 0.99,
                  "tags": [
                    "hardware",
                    "fasteners"
                  ]
                }
              ],
              "clients": {
                "data": {
                  "name": "Google",
                  "hosts": [
                    "alpha",
                    "omega"
                  ]
                },
                "hosts": {
                  "alpha": "10.0.0.1",
                  "omega": "10.0.0.2"
                }
              },
              "servers": {
                "alpha": {
                  "ip": "10.0.0.1",
                  "dc": "eqdc10",
                  "country": "中国"
                },
                "beta": {
                  "ip": "10.0.0.2",
                  "dc": "eqdc10",
                  "country": "美国"
                }
              },
              "fruit": [
                {
                  "name": "apple",
                  "color": "red",
                  "shape": "round",
                  "variety": [
                    {
                      "name": "red delicious",
                      "sweetness": 8
                    },
                    {
                      "name": "granny smith",
                      "sweetness": 4
                    }
                  ]
                },
                {
                  "name": "banana",
                  "color": "yellow",
                  "shape": "curved",
                  "variety": [
                    {
                      "name": "cavendish",
                      "sweetness": 9
                    }
                  ]
                }
              ],
              "special_values": {
                "empty_string": "",
                "zero_integer": 0,
                "zero_float": 0,
                "positive_infinity": 0,
                "negative_infinity": 0,
                "not_a_number": 0,
                "true_boolean": true,
                "false_boolean": false,
                "empty_array": [],
                "empty_inline_table": {},
                "commented_key1": "value",
                "commented_key2": "value",
                "commented_key3": "with value",
                "commented_key4": "with value",
                "commented_array1": [
                  0,
                  1,
                  2,
                  3,
                  4,
                  5
                ],
                "commented_array2": [
                  1,
                  2,
                  3
                ],
                "commented_table": {
                  "value": 1
                }
              },
              "config": {
                "version": "1.0.0",
                "debug": true,
                "max_connections": 100,
                "timeout": 30.5,
                "logging": {
                  "level": "info",
                  "file": "/var/log/app.log",
                  "rotate": true,
                  "max_size": "10MB"
                },
                "features": {
                  "authentication": true,
                  "caching": false,
                  "compression": true
                },
                "endpoints": [
                  {
                    "name": "api",
                    "url": "https://api.example.com",
                    "methods": [
                      "GET",
                      "POST",
                      "PUT",
                      "DELETE"
                    ],
                    "timeout": 5000
                  },
                  {
                    "name": "webhook",
                    "url": "https://webhook.example.com",
                    "methods": [
                      "POST"
                    ],
                    "timeout": 3000
                  }
                ]
              },
              "i18n": {
                "english": "Hello, World!",
                "chinese": "你好，世界！",
                "japanese": "こんにちは、世界！",
                "korean": "안녕하세요, 세계!",
                "arabic": "مرحبا بالعالم!",
                "russian": "Привет, мир!",
                "emoji": "🌍🌎🌏"
              }
            }

            """;

    public static string ClassData =>
        """
            /// <summary>
            /// TOML 测试数据 - 覆盖所有数据类型和格式
            /// <para>基于 TOML v1.0.0 规范</para>
            /// </summary>
            public class ExampleObject : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                /// <summary>
                /// ============ 基本字符串 ============
                /// </summary>
                [TomlPropertyOrder(0)]
                [TomlPropertyName("title")]
                public string Title { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("description")]
                public string Description { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("basic_string")]
                public string BasicString { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("unicode_string")]
                public string UnicodeString { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("empty_string")]
                public string EmptyString { get; set; }

                /// <summary>
                /// ============ 多行字符串 ============
                /// </summary>
                [TomlPropertyOrder(5)]
                [TomlPropertyName("multiline_basic1")]
                public string MultilineBasic1 { get; set; }

                [TomlPropertyOrder(6)]
                [TomlPropertyName("multiline_basic2")]
                public string MultilineBasic2 { get; set; }

                [TomlPropertyOrder(7)]
                [TomlPropertyName("multiline_literal")]
                public string MultilineLiteral { get; set; }

                /// <summary>
                /// ============ 字面字符串 ============
                /// </summary>
                [TomlPropertyOrder(8)]
                [TomlPropertyName("literal_string")]
                public string LiteralString { get; set; }

                [TomlPropertyOrder(9)]
                [TomlPropertyName("windows_path")]
                public string WindowsPath { get; set; }

                [TomlPropertyOrder(10)]
                [TomlPropertyName("regex_pattern")]
                public string RegexPattern { get; set; }

                /// <summary>
                /// ============ 转义字符串 ============
                /// </summary>
                [TomlPropertyOrder(11)]
                [TomlPropertyName("escaped_string")]
                public string EscapedString { get; set; }

                [TomlPropertyOrder(12)]
                [TomlPropertyName("unicode_escape")]
                public string UnicodeEscape { get; set; }

                /// <summary>
                /// ============ 整数 ============
                /// </summary>
                [TomlPropertyOrder(13)]
                [TomlPropertyName("positive_int")]
                public int PositiveInt { get; set; }

                [TomlPropertyOrder(14)]
                [TomlPropertyName("negative_int")]
                public int NegativeInt { get; set; }

                [TomlPropertyOrder(15)]
                [TomlPropertyName("zero_int")]
                public int ZeroInt { get; set; }

                [TomlPropertyOrder(16)]
                [TomlPropertyName("underscore_int")]
                public int UnderscoreInt { get; set; }

                [TomlPropertyOrder(17)]
                [TomlPropertyName("hex_int")]
                public long HexInt { get; set; }

                [TomlPropertyOrder(18)]
                [TomlPropertyName("octal_int")]
                public int OctalInt { get; set; }

                [TomlPropertyOrder(19)]
                [TomlPropertyName("binary_int")]
                public int BinaryInt { get; set; }

                /// <summary>
                /// ============ 浮点数 ============
                /// </summary>
                [TomlPropertyOrder(20)]
                [TomlPropertyName("positive_float")]
                public double PositiveFloat { get; set; }

                [TomlPropertyOrder(21)]
                [TomlPropertyName("negative_float")]
                public double NegativeFloat { get; set; }

                [TomlPropertyOrder(22)]
                [TomlPropertyName("exponent_float")]
                public double ExponentFloat { get; set; }

                [TomlPropertyOrder(23)]
                [TomlPropertyName("negative_exponent")]
                public double NegativeExponent { get; set; }

                [TomlPropertyOrder(24)]
                [TomlPropertyName("underscore_float")]
                public double UnderscoreFloat { get; set; }

                [TomlPropertyOrder(25)]
                [TomlPropertyName("infinity")]
                public double Infinity { get; set; }

                [TomlPropertyOrder(26)]
                [TomlPropertyName("negative_infinity")]
                public double NegativeInfinity { get; set; }

                [TomlPropertyOrder(27)]
                [TomlPropertyName("not_a_number")]
                public double NotANumber { get; set; }

                /// <summary>
                /// ============ 布尔值 ============
                /// </summary>
                [TomlPropertyOrder(28)]
                [TomlPropertyName("is_enabled")]
                public bool IsEnabled { get; set; }

                [TomlPropertyOrder(29)]
                [TomlPropertyName("is_disabled")]
                public bool IsDisabled { get; set; }

                /// <summary>
                /// ============ 日期时间 ============
                /// <para>RFC 3339 格式</para>
                /// </summary>
                [TomlPropertyOrder(30)]
                [TomlPropertyName("offset_datetime")]
                public DateTimeOffset OffsetDatetime { get; set; }

                [TomlPropertyOrder(31)]
                [TomlPropertyName("local_datetime")]
                public DateTime LocalDatetime { get; set; }

                [TomlPropertyOrder(32)]
                [TomlPropertyName("local_date")]
                public DateTime LocalDate { get; set; }

                [TomlPropertyOrder(33)]
                [TomlPropertyName("local_time")]
                public DateTime LocalTime { get; set; }

                /// <summary>
                /// 带毫秒的日期时间
                /// </summary>
                [TomlPropertyOrder(34)]
                [TomlPropertyName("precise_datetime")]
                public DateTimeOffset PreciseDatetime { get; set; }

                [TomlPropertyOrder(35)]
                [TomlPropertyName("timezone_datetime")]
                public DateTimeOffset TimezoneDatetime { get; set; }

                /// <summary>
                /// ============ 数组 ============
                /// </summary>
                [TomlPropertyOrder(36)]
                [TomlPropertyName("simple_array")]
                public List<int> SimpleArray { get; set; }

                [TomlPropertyOrder(37)]
                [TomlPropertyName("string_array")]
                public List<string> StringArray { get; set; }

                [TomlPropertyOrder(38)]
                [TomlPropertyName("nested_array")]
                public List<List<TomlNode>> NestedArray { get; set; }

                [TomlPropertyOrder(39)]
                [TomlPropertyName("mixed_array")]
                public List<TomlNode> MixedArray { get; set; }

                [TomlPropertyOrder(40)]
                [TomlPropertyName("empty_array")]
                public List<TomlNode> EmptyArray { get; set; }

                /// <summary>
                /// 多行数组
                /// </summary>
                [TomlPropertyOrder(41)]
                [TomlPropertyName("multiline_array")]
                public List<int> MultilineArray { get; set; }

                /// <summary>
                /// 数组中的数组
                /// </summary>
                [TomlPropertyOrder(42)]
                [TomlPropertyName("array_of_arrays")]
                public List<List<TomlNode>> ArrayOfArrays { get; set; }

                [TomlPropertyOrder(43)]
                [TomlPropertyName("products")]
                public List<ProductsAnonymousClass> Products { get; set; }

                [TomlPropertyOrder(44)]
                [TomlPropertyName("fruit")]
                public List<FruitAnonymousClass> Fruit { get; set; }

                /// <summary>
                /// ============ 表 ============
                /// </summary>
                [TomlPropertyOrder(45)]
                [TomlPropertyName("database")]
                public DatabaseClass Database { get; set; }

                /// <summary>
                /// ============ 嵌套表 ============
                /// </summary>
                [TomlPropertyOrder(46)]
                [TomlPropertyName("clients")]
                public ClientsClass Clients { get; set; }

                /// <summary>
                /// ============ 复杂嵌套结构 ============
                /// </summary>
                [TomlPropertyOrder(47)]
                [TomlPropertyName("servers")]
                public ServersClass Servers { get; set; }

                /// <summary>
                /// ============ 特殊值测试 ============
                /// </summary>
                [TomlPropertyOrder(48)]
                [TomlPropertyName("special_values")]
                public SpecialValuesClass SpecialValues { get; set; }

                /// <summary>
                /// ============ 复杂数据结构 ============
                /// </summary>
                [TomlPropertyOrder(49)]
                [TomlPropertyName("config")]
                public ConfigClass Config { get; set; }

                /// <summary>
                /// ============ 国际化测试 ============
                /// </summary>
                [TomlPropertyOrder(50)]
                [TomlPropertyName("i18n")]
                public I18nClass I18n { get; set; }

                /// <summary>
                /// ============ 数据类型边界测试 ============
                /// </summary>
                [TomlPropertyOrder(51)]
                [TomlPropertyName("boundaries")]
                public BoundariesClass Boundaries { get; set; }

                /// <summary>
                /// ============ 引用和转义测试 ============
                /// </summary>
                [TomlPropertyOrder(52)]
                [TomlPropertyName("quotes_and_escapes")]
                public QuotesAndEscapesClass QuotesAndEscapes { get; set; }
            }

            public class ProductsAnonymousClass
            {
                [TomlPropertyOrder(0)]
                [TomlPropertyName("name")]
                public string Name { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("sku")]
                public int Sku { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("price")]
                public double Price { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("tags")]
                public List<string> Tags { get; set; }
            }

            public class FruitAnonymousClass
            {
                [TomlPropertyOrder(0)]
                [TomlPropertyName("name")]
                public string Name { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("color")]
                public string Color { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("shape")]
                public string Shape { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("variety")]
                public List<VarietyAnonymousClass> Variety { get; set; }
            }

            public class VarietyAnonymousClass
            {
                [TomlPropertyOrder(0)]
                [TomlPropertyName("name")]
                public string Name { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("sweetness")]
                public int Sweetness { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("inline_table")]
                public InlineTableClass InlineTable { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("empty_inline_table")]
                public EmptyInlineTableClass EmptyInlineTable { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("nested_inline")]
                public NestedInlineClass NestedInline { get; set; }
            }

            /// <summary>
            /// ============ 内联表 ============
            /// </summary>
            public class InlineTableClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("name")]
                public string Name { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("age")]
                public int Age { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("city")]
                public string City { get; set; }
            }

            public class EmptyInlineTableClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();


            }

            public class NestedInlineClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("point")]
                public PointClass Point { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("color")]
                public string Color { get; set; }
            }

            public class PointClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("x")]
                public int X { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("y")]
                public int Y { get; set; }
            }

            /// <summary>
            /// ============ 表 ============
            /// </summary>
            public class DatabaseClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("server")]
                public string Server { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("ports")]
                public List<int> Ports { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("connection_max")]
                public int ConnectionMax { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("enabled")]
                public bool Enabled { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("credentials")]
                public CredentialsClass Credentials { get; set; }
            }

            public class CredentialsClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("username")]
                public string Username { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("password")]
                public string Password { get; set; }
            }

            /// <summary>
            /// ============ 嵌套表 ============
            /// </summary>
            public class ClientsClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("data")]
                public DataClass Data { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("hosts")]
                public HostsClass Hosts { get; set; }
            }

            public class DataClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("name")]
                public string Name { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("hosts")]
                public List<string> Hosts { get; set; }
            }

            public class HostsClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("alpha")]
                public string Alpha { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("omega")]
                public string Omega { get; set; }
            }

            /// <summary>
            /// ============ 复杂嵌套结构 ============
            /// </summary>
            public class ServersClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("alpha")]
                public AlphaClass Alpha { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("beta")]
                public BetaClass Beta { get; set; }
            }

            public class AlphaClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("ip")]
                public string Ip { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("dc")]
                public string Dc { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("country")]
                public string Country { get; set; }
            }

            public class BetaClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("ip")]
                public string Ip { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("dc")]
                public string Dc { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("country")]
                public string Country { get; set; }
            }

            /// <summary>
            /// ============ 特殊值测试 ============
            /// </summary>
            public class SpecialValuesClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("empty_string")]
                public string EmptyString { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("zero_integer")]
                public int ZeroInteger { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("zero_float")]
                public int ZeroFloat { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("positive_infinity")]
                public double PositiveInfinity { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("negative_infinity")]
                public double NegativeInfinity { get; set; }

                [TomlPropertyOrder(5)]
                [TomlPropertyName("not_a_number")]
                public double NotANumber { get; set; }

                [TomlPropertyOrder(6)]
                [TomlPropertyName("true_boolean")]
                public bool TrueBoolean { get; set; }

                [TomlPropertyOrder(7)]
                [TomlPropertyName("false_boolean")]
                public bool FalseBoolean { get; set; }

                [TomlPropertyOrder(8)]
                [TomlPropertyName("empty_array")]
                public List<TomlNode> EmptyArray { get; set; }

                /// <summary>
                /// ============ 注释测试 ============
                /// <para>这是一个注释</para>
                /// </summary>
                [TomlPropertyOrder(9)]
                [TomlPropertyName("commented_key1")]
                public string CommentedKey1 { get; set; }

                [TomlPropertyOrder(10)]
                [TomlPropertyName("commented_key2")]
                public string CommentedKey2 { get; set; }

                /// <summary>
                /// 多行注释
                /// <para>可以有多行</para>
                /// <para>每行都以 # 开头</para>
                /// </summary>
                [TomlPropertyOrder(11)]
                [TomlPropertyName("commented_key3")]
                public string CommentedKey3 { get; set; }

                /// <summary>
                /// 注释
                /// <para>可以有多行</para>
                /// </summary>
                [TomlPropertyOrder(12)]
                [TomlPropertyName("commented_key4")]
                public string CommentedKey4 { get; set; }

                /// <summary>
                /// 这是一个带有注释的数组
                /// <para>可以在数组末尾注释</para>
                /// </summary>
                [TomlPropertyOrder(13)]
                [TomlPropertyName("commented_array1")]
                public List<int> CommentedArray1 { get; set; }

                /// <summary>
                /// 单行数组可以有行尾注释
                /// </summary>
                [TomlPropertyOrder(14)]
                [TomlPropertyName("commented_array2")]
                public List<int> CommentedArray2 { get; set; }

                [TomlPropertyOrder(15)]
                [TomlPropertyName("empty_inline_table")]
                public EmptyInlineTableClass EmptyInlineTable { get; set; }

                /// <summary>
                /// 单行表格可以有行尾注释
                /// </summary>
                [TomlPropertyOrder(16)]
                [TomlPropertyName("commented_table")]
                public CommentedTableClass CommentedTable { get; set; }
            }

            /// <summary>
            /// 单行表格可以有行尾注释
            /// </summary>
            public class CommentedTableClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("value")]
                public int Value { get; set; }
            }

            /// <summary>
            /// ============ 复杂数据结构 ============
            /// </summary>
            public class ConfigClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("version")]
                public string Version { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("debug")]
                public bool Debug { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("max_connections")]
                public int MaxConnections { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("timeout")]
                public double Timeout { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("endpoints")]
                public List<EndpointsAnonymousClass> Endpoints { get; set; }

                [TomlPropertyOrder(5)]
                [TomlPropertyName("logging")]
                public LoggingClass Logging { get; set; }

                [TomlPropertyOrder(6)]
                [TomlPropertyName("features")]
                public FeaturesClass Features { get; set; }
            }

            public class EndpointsAnonymousClass
            {
                [TomlPropertyOrder(0)]
                [TomlPropertyName("name")]
                public string Name { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("url")]
                public string Url { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("methods")]
                public List<string> Methods { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("timeout")]
                public int Timeout { get; set; }
            }

            public class LoggingClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("level")]
                public string Level { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("file")]
                public string File { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("rotate")]
                public bool Rotate { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("max_size")]
                public string MaxSize { get; set; }
            }

            public class FeaturesClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("authentication")]
                public bool Authentication { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("caching")]
                public bool Caching { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("compression")]
                public bool Compression { get; set; }
            }

            /// <summary>
            /// ============ 国际化测试 ============
            /// </summary>
            public class I18nClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("english")]
                public string English { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("chinese")]
                public string Chinese { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("japanese")]
                public string Japanese { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("korean")]
                public string Korean { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("arabic")]
                public string Arabic { get; set; }

                [TomlPropertyOrder(5)]
                [TomlPropertyName("russian")]
                public string Russian { get; set; }

                [TomlPropertyOrder(6)]
                [TomlPropertyName("emoji")]
                public string Emoji { get; set; }
            }

            /// <summary>
            /// ============ 数据类型边界测试 ============
            /// </summary>
            public class BoundariesClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("max_int")]
                public long MaxInt { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("min_int")]
                public long MinInt { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("small_float")]
                public double SmallFloat { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("large_float")]
                public long LargeFloat { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("very_long_string")]
                public string VeryLongString { get; set; }
            }

            /// <summary>
            /// ============ 引用和转义测试 ============
            /// </summary>
            public class QuotesAndEscapesClass : ITomlObjectComment
            {
                /// <inheritdoc/>
                public TomlComment ObjectComment { get; set; } = new();
                /// <inheritdoc/>
                public Dictionary<string, TomlComment> PropertyComments { get; set; } = new();

                [TomlPropertyOrder(0)]
                [TomlPropertyName("single_quote")]
                public string SingleQuote { get; set; }

                [TomlPropertyOrder(1)]
                [TomlPropertyName("double_quote")]
                public string DoubleQuote { get; set; }

                [TomlPropertyOrder(2)]
                [TomlPropertyName("backslash")]
                public string Backslash { get; set; }

                [TomlPropertyOrder(3)]
                [TomlPropertyName("newline")]
                public string Newline { get; set; }

                [TomlPropertyOrder(4)]
                [TomlPropertyName("tab")]
                public string Tab { get; set; }

                [TomlPropertyOrder(5)]
                [TomlPropertyName("carriage_return")]
                public string CarriageReturn { get; set; }

                [TomlPropertyOrder(6)]
                [TomlPropertyName("unicode")]
                public string Unicode { get; set; }
            }
            """;
}
