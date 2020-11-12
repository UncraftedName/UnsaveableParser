import re
import win32clipboard

empty = "^[ \t]*$"
end = r"[ \t]*END_DATADESC\(\)"
commentRe = r"/{2,} *(?P<comment>.*)"

beginRe = (
    r"^ *BEGIN_SIMPLE_DATADESC\( *(?P<name>\S+) *\)",
    r"^ *BEGIN_DATADESC_NO_BASE\( *(?P<name>\S+) *\)",
    r"^ *BEGIN_DATADESC\( *(?P<name>\S+) *\)"
)

keyFieldRe = r'^ *DEFINE_KEYFIELD\( *(?P<name>\S+?) *, *FIELD_(?P<type>\S+?), *"(?P<mapname>\S+?)" *[),]'
inputRe = r'^ *DEFINE_INPUT\( *(?P<name>\S+?) *, *FIELD_(?P<type>\S+?), *"(?P<inputname>\S+?)" *[),]'
globalKeyFieldRe = r'^ *DEFINE_GLOBAL_KEYFIELD\( *(?P<name>\S+?) *, *FIELD_(?P<type>\S+?), *"(?P<mapname>\S+?)" *[),]'
outputRe = r'^ *DEFINE_OUTPUT *\( *(?P<name>\S+?) *, *"(?P<outputname>\S+?)" *[),]'
fieldRe = r"^ *DEFINE_(?P<fieldtype>[\S]*?)\( *(?P<name>\S+?) *, *FIELD_(?P<type>\S+?) *[),]"
fieldReAlt = r'^ *DEFINE_(?P<fieldtype>[\S]*?)\( *FIELD_(?P<type>\S+?) *, *"(?P<name>\S+?)" *[),]'
funcRe = '^ *DEFINE_FUNCTION\( *(?P<name>\S+?) *[),]'
inputFuncRe = '^ *DEFINE_INPUTFUNC\( *FIELD_(?P<type>\S+?) *, *"(?P<name>\S+?)" *, *(?P<func>\S+?) *[),]'
thinkFuncRe = '^ *DEFINE_THINKFUNC\( *(?P<func>\S+?) *[),]'

win32clipboard.OpenClipboard()
data = win32clipboard.GetClipboardData()
win32clipboard.CloseClipboard()
data = data.replace("\t", "").replace("\r", "")
lines = re.split(r"\n", data)

output = []

for line in lines:
    line = line.replace("#", "//#")
    if re.match(empty, line):
        continue
    m = re.match(commentRe, line)
    if m:
        comment = " // " + m.group("comment")
    else:
        comment = ""
    beginMatch = None
    for r in beginRe:
        m = re.match(r, line)
        if m:
            beginMatch = m
    if beginMatch:
        output.append('\t\t\tBeginDataMap("' + beginMatch.group("name") + '");' + comment)
        continue
    m = re.match(fieldRe, line)
    if m is None:
        m = re.match(outputRe, line)
        if m is None:
            m = re.match(funcRe, line)
            if m is None:
                m = re.match(thinkFuncRe, line)
                if m is None:
                    m = re.match(fieldReAlt, line)
                else:
                    output.append('\t\t\tDefineThinkFunc("' + m.group("func") + '");' + comment)
                    continue
            else:
                output.append('\t\t\tDefineFunction("' + m.group("name") + '");' + comment)
                continue
        else:
            output.append('\t\t\tDefineOutput("' + m.group("name") + '", "' + m.group("outputname") + '");' + comment)
            continue
    if m:
        s = "\t\t\tDefine"
        f = m.group("fieldtype")
        if f == "FIELD":
            s += "Field"
        elif f == "KEYFIELD" or f == "GLOBAL_KEYFIELD":
            if f == "KEYFIELD":
                s += 'KeyField("'
                m = re.match(keyFieldRe, line)
            else:
                s += 'GlobalKeyField("'
                m = re.match(globalKeyFieldRe, line)
            output.append(s + m.group("name") + '", "' + m.group("mapname") + '", ' + m.group("type") + ');' + comment)
            continue
        elif f == "INPUT":
            m = re.match(inputRe, line)
            output.append('\t\t\tDefineInput("' + m.group("name") + '", "' + m.group("inputname") + '", ' + m.group("type") + ');' + comment)
            continue
        elif f == "INPUTFUNC":
            m = re.match(inputFuncRe, line)
            output.append('\t\t\tDefineInputFunc("' + m.group("name") + '", "' + m.group("func") + '", ' + m.group("type") + ');' + comment)
            continue
        else:
            s += f
        s += '("'
        output.append(s + m.group("name") + '", ' + m.group("type") + ');' + comment)
        continue
    m = re.match(end, line)
    if m:
        output.append("\t\t\t" + comment)
        continue
    output.append(line)

print("input, (" + str(len(lines)) + " lines):")
print("\n".join(lines))
print("\n\n...........output.............:\n")
print("\n".join(output))

input()

win32clipboard.OpenClipboard()
win32clipboard.EmptyClipboard()
win32clipboard.SetClipboardText("\n".join(output))
win32clipboard.CloseClipboard()
