BIN_DIR = bin/

serverapp: server/*.cs server/*/*.cs util/*.cs
	csc $^ -out:$(BIN_DIR)/$@

clientapp: client/*.cs client/*/*.cs util/*.cs
	csc $^ -out:$(BIN_DIR)/$@
