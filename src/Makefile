exe:
	make -C libgtest-report flo
	make -C gtest-report-print exe
	make -C gtest-report-html exe

all:
	make -C libgtest-report all
	make -C gtest-report-print all
	make -C gtest-report-html all

dll:
	make -C libgtest-report dll

flo:
	make -C libgtest-report flo
	make -C gtest-report-print flo
	make -C gtest-report-html flo

nuget:
	nuget install DotArguments -version 0.3.1 -outputdirectory packages
	nuget install HtmlAgilityPack -version 1.4.9.5 -outputdirectory packages

clean:
	make -C libgtest-report clean
	make -C gtest-report-print clean
	make -C gtest-report-html clean
