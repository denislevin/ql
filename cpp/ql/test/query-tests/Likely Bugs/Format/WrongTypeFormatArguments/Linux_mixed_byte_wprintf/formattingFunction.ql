import cpp

from FormattingFunction f
select f, concat(f.getFormatCharType().toString(), ", "),
  concat(f.getDefaultCharType().toString(), ", "),
  concat(f.getNonDefaultCharType().toString(), ", "), concat(f.getWideCharType().toString(), ", ")
