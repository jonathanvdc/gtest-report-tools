files = $(shell find . -name '*.cs')

common_options = -asm-name=libgtest-report -asm-version=0.1.0.0 -Wall -Wextra -force-write -rt-libs System.Xml

all: dll flo

dll: bin/clr/libgtest-report.dll
flo: bin/flo/libgtest-report.flo

clean:
	rm -rf bin/
	rm -rf obj/

bin/clr/libgtest-report.dll: $(files)
	ecsc $(files) -platform clr -o $@ -O3 $(common_options)

bin/flo/libgtest-report.flo: $(files)
	ecsc $(files) -platform ir -runtime clr -o $@ $(common_options)
