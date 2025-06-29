# Pip installs:
## pip install pdf2image
## pip install pyinstaller

import sys
import os
from pdf2image import convert_from_path

def convert(pdf_path, output_dir):
    if getattr(sys, 'frozen', False):
        base_path = sys._MEIPASS
    else:
        base_path = os.path.dirname(os.path.abspath(__file__))

    poppler_path = os.path.join(base_path, "poppler-24.08.0", "Library", "bin")

    print(f"Using poppler_path: {poppler_path}")
    
    pages = convert_from_path(
        pdf_path,
        dpi=200,
        poppler_path=poppler_path
    )

    os.makedirs(output_dir, exist_ok=True)
    for i, page in enumerate(pages):
        page.save(os.path.join(output_dir, f"page-{i+1:03d}.png"), "PNG")

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: pdf_to_img.exe <input.pdf> <output_folder>")
        sys.exit(1)

    convert(sys.argv[1], sys.argv[2])
    print("PDF conversion complete.")
