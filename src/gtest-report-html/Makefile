files = $(shell find . -name '*.cs') \
	../libgtest-report/bin/flo/libgtest-report.flo

libs = ../packages/DotArguments.0.3.1/lib/DotArguments.dll \
	../packages/HtmlAgilityPack.1.4.9.5/lib/Net45/HtmlAgilityPack.dll

common_options = -asm-name=gtest-report-html -asm-version=0.1.2.0 -Wall -Wextra -force-write -fno-whole-program -rt-libs System.Xml -libs $(libs)

all: exe flo

exe: bin/clr/gtest-report-html.exe
flo: bin/flo/gtest-report-html.flo

clean:
	rm -rf bin/
	rm -rf obj/

bin/clr/gtest-report-html.exe: $(files)
	ecsc $(files) -platform clr -o $@ -O3 $(common_options)

bin/flo/gtest-report-html.flo: $(files)
	ecsc $(files) -platform ir -runtime clr -o $@ $(common_options)
