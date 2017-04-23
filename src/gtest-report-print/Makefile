files = $(shell find . -name '*.cs') \
		../libgtest-report/bin/flo/libgtest-report.flo

common_options = -asm-name=gtest-report-print -asm-version=0.1.2.0 -Wall -Wextra -force-write -rt-libs System.Xml

all: exe flo

exe: bin/clr/gtest-report-print.exe
flo: bin/flo/gtest-report-print.flo

clean:
	rm -rf bin/
	rm -rf obj/

bin/clr/gtest-report-print.exe: $(files)
	ecsc $(files) -platform clr -o $@ -O3 $(common_options)

bin/flo/gtest-report-print.flo: $(files)
	ecsc $(files) -platform ir -runtime clr -o $@ $(common_options)
